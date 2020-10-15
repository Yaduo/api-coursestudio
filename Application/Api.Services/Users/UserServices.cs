using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using MediatR;
using AutoMapper;
using CourseStudio.Application.Dtos.Users;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Application.Events.Identities;
using CourseStudio.Application.Common.Helpers;
using CourseStudio.Application.Common;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Configs;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Lib.Utilities.Http;

namespace CourseStudio.Api.Services.Users
{
	public class UserServices: BaseService, IUserServices
    {
		private readonly ICourseRepository _courseRepository;
		private readonly IStudyRecordRepository _studyRecordRepository;
		private readonly StorageConnectionConfig _storageConnectionConfig;
		private readonly PaymentProcessConfig _paymentProcessConfigs;

        public UserServices(
			ICourseRepository courseRepository,
			IStudyRecordRepository studyRecordRepository,
			IOptions<StorageConnectionConfig> storageConnectionConfig,
			IOptions<PaymentProcessConfig> paymentProcessConfigs,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_courseRepository = courseRepository;
			_studyRecordRepository = studyRecordRepository;
			_storageConnectionConfig = storageConnectionConfig.Value;
			_paymentProcessConfigs = paymentProcessConfigs.Value;
        }

		public async Task<UserDto> GetCurrentUserAsync() 
		{
			// because need to allow inActivated user to login
            // so only throw exception for access blocked user
			var user = await IdentityHelper.GetUnblockedUser(_httpContextAccessor, _userRepository, _userManager);
			return Mapper.Map<UserDto>(user);
		}

		public async Task<UserDto> UpdateUserProfileAsync(UserUpdateFormRequestDto userProfileUpdateDto) 
		{
			// 1. get current user
            var user = await GetCurrentUser();
			if (user == null)
            {
                throw new NotFoundException("user not found");
            }
            // 3. update user
            string avararUrl = null;
            if (userProfileUpdateDto.ProfileImage != null)
            {
                avararUrl = _storageConnectionConfig.Url + _storageConnectionConfig.AvatarContainerName + "/" + userProfileUpdateDto.ProfileImage;
            }
            user.UpdateProfile(userProfileUpdateDto.FirstName, userProfileUpdateDto.LastName, avararUrl);
            // 4. apply db change
			await _userRepository.SaveAsync();
            return Mapper.Map<UserDto>(user);
        }

		public async Task UpdateEmailForCurrentUserAsync(string newEmail) 
		{
			// 1. get current user
			var user = await GetCurrentUser(false);
			if (user == null)
            {
                throw new NotFoundException("user not found");
            }
			// 2. update user
			await user.UpdateEmailAsync(_userManager, newEmail);
			// 3. apply db change
            await _userRepository.SaveAsync();
			// 4. send comfiremation email
			await _mediator.PublishAsync(new NewUserRegisteredEvent(user.Id));
		}

		public async Task<CreditCardDto> GetPaymentProfileAsync()
		{
			var user = await GetCurrentUser();
            if (user == null)
            {
                throw new NotFoundException("user not found");
            }
			if (user.PaymentProfile == null) 
			{
				throw new NotFoundException("user payment profile not found");
			}
            
            // Due to the CPI regulation, credit card information not allow to store in our database
            // send request to vendor and get the credit card info
			var url = string.Format(_paymentProcessConfigs.GetProfileUrl, _paymentProcessConfigs.RootUrl, user.PaymentProfile.CustomerCode);
			var authorization = PaymentHelper.GetAuthorizationHeader(_paymentProcessConfigs.ProfilePasscode);
			var headers = new List<(string, string)> { authorization };
			var response = await HttpRequestHelper.GetAsync(url, headers);
			var meta = await response.Content.ReadAsStringAsync();
			PaymentProfileResponseDto paymentProfileDto = JsonConvert.DeserializeObject<PaymentProfileResponseDto>(meta);

			return paymentProfileDto.Card;
		}

		public async Task<UserDto> CreatePaymentProfileAsync(string cardHolderName, string paymentToken) 
		{
            // 1. get user
			var user = await GetCurrentUser();
			if(user == null) 
			{
				throw new NotFoundException("user not found");
			}
            
			// 2. Get Bambora Payment Profile
			var createProfileUrl = string.Format(_paymentProcessConfigs.CreateProfileUrl, _paymentProcessConfigs.RootUrl);
            // post to bambora to create profile
			var response = await HttpRequestHelper.PostAsync(
				createProfileUrl, 
				new List<(string, string)> { PaymentHelper.GetAuthorizationHeader(_paymentProcessConfigs.ProfilePasscode) }, 
				PaymentHelper.CreatePaymenProfileRequestBody(cardHolderName, paymentToken)
			);
            // check respons
			var jsonStr = await response.Content.ReadAsStringAsync();
			if (response.StatusCode != HttpStatusCode.OK)
            {
				throw new BadRequestException(jsonStr);
            }
			// TODO: exception handler
			PaymentProfileCreateResponseDto profileCreateData = JsonConvert.DeserializeObject<PaymentProfileCreateResponseDto>(jsonStr);
          
            // 3. Create user payment profile in database
			user.CreatePaymentProfile(profileCreateData.Customer_code, paymentToken);
            await _userRepository.SaveAsync();
			return Mapper.Map<UserDto>(user);
		}

		public async Task<UserDto> UpdatePaymentProfileAsync(string cardHolderName, string paymentToken)
        {
            var user = await GetCurrentUser();
            if (user == null)
            {
                throw new NotFoundException("user not found");
            }
			if (user.PaymentProfile == null) 
			{
				throw new NotFoundException("user payment method not found.");
			}

			// 2. Update Bambora Payment Profile
			var url = string.Format(_paymentProcessConfigs.UpdateProfileUrl, _paymentProcessConfigs.RootUrl, user.PaymentProfile.CustomerCode);
			var response = await HttpRequestHelper.PutAsync(
				url, 
                new List<(string, string)> { PaymentHelper.GetAuthorizationHeader(_paymentProcessConfigs.ProfilePasscode) }, 
                PaymentHelper.CreatePaymenProfileRequestBody(cardHolderName, paymentToken)
            );
            // check respons
            var jsonStr = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new BadRequestException(jsonStr);
            }
            // TODO: exception handler
            PaymentProfileCreateResponseDto profileCreateData = JsonConvert.DeserializeObject<PaymentProfileCreateResponseDto>(jsonStr);
          
            // 3. save result
            user.UpdatePaymentProfile(paymentToken);
            await _userRepository.SaveAsync();
            return Mapper.Map<UserDto>(user);
        }

		public async Task DeletePaymentProfileAsync()
        {
            var user = await GetCurrentUser();
            if (user == null)
            {
                throw new NotFoundException("user not found");
            }
            if (user.PaymentProfile == null)
            {
                throw new NotFoundException("user payment method not found.");
            }

            // 2. Update Bambora Payment Profile
			var url = string.Format(_paymentProcessConfigs.DeleteProfileUrl, _paymentProcessConfigs.RootUrl, user.PaymentProfile.CustomerCode);
			var response = await HttpRequestHelper.DeleteAsync(
                url,
                new List<(string, string)> { PaymentHelper.GetAuthorizationHeader(_paymentProcessConfigs.ProfilePasscode) },
				new {}
            );
            // check respons
            var jsonStr = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new BadRequestException(jsonStr);
            }

            // 3. save result
            user.DeletePaymentProfile();
            await _userRepository.SaveAsync();
        }

		public async Task<PaginationDto<StudyRecordDto>> GetPagedStudyHistoryAsync(int pageNumber, int pageSize)
		{
			var user = await GetCurrentUser();
			return Mapper.Map<PaginationDto<StudyRecordDto>>(await _studyRecordRepository.GetPagedStudyRecordByUserIdAsync(user.Id, pageNumber, pageSize));
		}

		public async Task<StudyRecordDto> GetStudyHistoryAsync(int courseId) 
		{
			var user = await GetCurrentUser();
			return Mapper.Map<StudyRecordDto>(await _studyRecordRepository.GetStudyRecordByUserandCourseAsync(user.Id, courseId));
		}

		public async Task<StudyRecordDto> UpdateStudyHistoryAsync(int courseId, int lectureId) 
		{
			var user = await GetCurrentUser();
            
			var course = await _courseRepository.GetCourseAsync(courseId);
			if(course == null) 
			{
				throw new NotFoundException("course not found.");
			}

			var studyRecord = await _studyRecordRepository.GetStudyRecordByUserandCourseAsync(user.Id, courseId);
			if(studyRecord == null)
			{
				studyRecord = StudyRecord.Create(course, lectureId);
				user.StudyHistory.Add(studyRecord);
			}
			else 
			{
				studyRecord.Update(lectureId);
			}

			await _userRepository.SaveAsync();
			return Mapper.Map<StudyRecordDto>(studyRecord);
		}

        public async Task ApplyTutorForCurrentUserAsync(string resume) 
        {
            var user = await GetCurrentUser();
            user.ApplyTutor(resume, 0.5);
            await _userRepository.SaveAsync();
            await _mediator.PublishAsync(new ApplyTutorEvent(user.Id, resume));
        }

    }
}
