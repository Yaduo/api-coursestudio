using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common.ModelBinders;

namespace CourseStudio.Presentation.Common
{
	public class BaseController: Controller
    {
        public readonly ILogger _logger;
        private readonly IUrlHelper _urlHelper;

        public BaseController(ILogger logger, IUrlHelper urlHelper)
        {
            _logger = logger;
            _urlHelper = urlHelper;
        }

        protected string GeneratePaginationMetadata(int totalCount, int totalPages, int pageSize, int currentPage)
        {
            var paginationMetadata = new
            {
                totalCount,
                totalPages,
                pageSize,
                currentPage,
                previousPage = GenerateResourceUri(currentPage, pageSize, totalPages, PaginationUriType.PreviousPage),
                nextPage = GenerateResourceUri(currentPage, pageSize, totalPages, PaginationUriType.NextPage)
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata);
        }

        private string GenerateResourceUri(int pageNumber, int pageSize, int totalPages, PaginationUriType pagingType)
        {
            switch (pagingType)
            {
                case PaginationUriType.PreviousPage:
                    return pageNumber > 1 ? _urlHelper.Link("GetCourses", new
                    {
                        pageNumber = pageNumber - 1,
                        pageSize
                    }) : null;
                case PaginationUriType.NextPage:
                    return pageNumber < totalPages ? _urlHelper.Link("GetCourses", new
                    {
                        pageNumber = pageNumber + 1,
                        pageSize
                    }) : null;
                default:
                    return _urlHelper.Link("GetCourses", new
                    {
                        pageNumber,
                        pageSize
                    });
            }
        }
    }
}
