using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Users;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Domain.TraversalModel.Identities;

namespace CourseStudioManager.Api.Services.Users
{
	public interface ITutorService
    {
		Task<PaginationDto<TutorDto>> GetTutorsAsync(string keywords, TutorStateEnum? state, int pageNumber, int pageSize);
		Task<TutorDto> GetTutorByIdAsync(int tutorId);
        Task<TutorDto> ApproveAsync(int tutorId, string note);
        Task<TutorDto> RejectAsync(int tutorId, string note);
        Task<TutorDto> DeactiveAsync(int tutorId, string note);
        Task<IList<TutorRevenueReportDto>> GetTutorRevenueReport(DateTime fromDate, DateTime toDate);
    }
}
