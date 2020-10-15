using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Application.Dtos.Users;

namespace CourseStudio.Api.Services.Users
{
    public interface IUserServices
    {
		Task<UserDto> GetCurrentUserAsync();
        Task<UserDto> UpdateUserProfileAsync(UserUpdateFormRequestDto userProfileUpdateDto);
		Task UpdateEmailForCurrentUserAsync(string newEmail);
        // user payment profile
		Task<CreditCardDto> GetPaymentProfileAsync();
		Task<UserDto> CreatePaymentProfileAsync(string cardHolderName, string paymentToken);
		Task<UserDto> UpdatePaymentProfileAsync(string cardHolderName, string paymentToken);
		Task DeletePaymentProfileAsync();
		// user study history
		Task<PaginationDto<StudyRecordDto>> GetPagedStudyHistoryAsync(int pageNumber, int pageSize);
		Task<StudyRecordDto> GetStudyHistoryAsync(int courseId);
        Task<StudyRecordDto> UpdateStudyHistoryAsync(int courseId, int lectureId);
        Task ApplyTutorForCurrentUserAsync(string resume);
    }
}
