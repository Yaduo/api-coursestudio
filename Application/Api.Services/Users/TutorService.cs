using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using CourseStudio.Application.Common;
using CourseStudio.Application.Dtos.Users;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Courses;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Domain.Repositories.Trades;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Domain.TraversalModel.Identities;

namespace CourseStudio.Api.Services.Users
{
	public class TutorService : BaseService, ITutorService
    {
		private readonly ITutorRepository _tutorRepository;
		private readonly ICourseRepository _courseRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;

        public TutorService(
			ITutorRepository tutorRepository,
			ICourseRepository courseRepository,
            ISalesOrderRepository salesOrderRepository,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_tutorRepository = tutorRepository;
			_courseRepository = courseRepository;
            _salesOrderRepository = salesOrderRepository;
        }

		public async Task<PaginationDto<TutorDto>> GetPagedTutorsAsync(string keywords, int pageNumber, int pageSize)
		{
			return Mapper.Map<PaginationDto<TutorDto>>(await _tutorRepository.GetPagedTutorsAsync(keywords, TutorStateEnum.Approved, pageNumber, pageSize));
		}

		public async Task<TutorDto> GetTutorByIdAsync(int tutorId) 
		{
            return Mapper.Map<TutorDto>(await _tutorRepository.GetTutorByIdAsync(tutorId)); 
		}

		public async Task<TutorDto> UpdateCurrentTutor(TutorUpdateRequestDto tutorDto) 
		{
			var user = await GetCurrentUser();
			user.Tutor.Update(tutorDto.Resume);
            await _tutorRepository.SaveAsync();
            return Mapper.Map<TutorDto>(user.Tutor);
        }

		public async Task<IList<TutorRevenueReportDto>> GetTutorRevenueReport(DateTime fromDate, DateTime toDate)
        {
			// 1. get user
            var user = await GetCurrentUser();
			if(user.Tutor == null) 
			{
				throw new NotFoundException("User is not a tutor");
			}
			// 2. get all released course for the tutor
			var tutorCourses = await _courseRepository.GetCoursesByTutorIdAsync(user.Tutor.Id, true);
			// 3. get tutor orders by courses and date ragne
			var courseIds = tutorCourses.Select(c => c.Id).ToList();
			var orders = await _salesOrderRepository.GetOrderByCourseIdAsync(courseIds, fromDate, toDate);
			// 4. generate revenue report
			var revenueReport = new List<TutorRevenueReportDto>();
			foreach(var order in orders) 
			{
				foreach(var item in order.OrderItems) 
				{
					var report = new TutorRevenueReportDto()
					{
						OrderNumber = order.OrderNumber,
						CreateDateUtc = order.CreateDateUTC,
						CourseTitle = item.Course.Title,
						OriginalPrice = item.PriceOriginal,
						Price = item.Price,
						Rate = user.Tutor.CommissionRate, 
						Revenue = (double)item.Price * user.Tutor.CommissionRate
					};
					revenueReport.Add(report);
				}
			}         
			return revenueReport;
        }
    }
}
