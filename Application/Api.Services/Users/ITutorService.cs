using System;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Users;
using CourseStudio.Application.Dtos.Pagination;
using System.Collections.Generic;

namespace CourseStudio.Api.Services.Users
{
    public interface ITutorService
    {
		Task<PaginationDto<TutorDto>> GetPagedTutorsAsync(string keywords, int pageNumber, int pageSize);
		Task<TutorDto> GetTutorByIdAsync(int tutorId);
		Task<TutorDto> UpdateCurrentTutor(TutorUpdateRequestDto tutor);
		Task<IList<TutorRevenueReportDto>> GetTutorRevenueReport(DateTime fromDate, DateTime toDate);
    }
}
