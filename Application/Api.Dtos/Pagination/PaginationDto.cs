using System.Collections.Generic;

namespace CourseStudio.Application.Dtos.Pagination
{
	public class PaginationDto<TDto>
	{
		public IList<TDto> Items { get; set; }
		public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
	}
}
