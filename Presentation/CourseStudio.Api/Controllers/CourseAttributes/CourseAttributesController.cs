using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseStudio.Presentation.Common.ModelBinders;
using CourseStudio.Presentation.Common;
using CourseStudio.Api.Services.CourseAttributes;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Api.Controllers.CourseAttributes
{
	[Produces("application/json")]
	[Route("api/attributes")]
	public class CourseAttributesController : BaseController
	{
		private readonly ICourseAttributeServices _courseAttributeServices;

        public CourseAttributesController(
			ICourseAttributeServices courseAttributeServices,
            ILogger<CourseAttributesController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
		{
			_courseAttributeServices = courseAttributeServices;
        }

        // GET api/attributes
        // Get api/attributes?parentAttributeIds=365846bf-7f7d-4710-9742-7f0fdfaf31a2
        [HttpGet]
		public async Task<IActionResult> GetCourseFilterAttributes([ModelBinder(BinderType = typeof(ArrayModelBinder))] IList<int?> parentAttributeIds)
		{
			try
			{
				var results = await _courseAttributeServices.GetEntityAttributeTypeAsync(parentAttributeIds);
				return Ok(results);
			}
			catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
			catch (Exception ex)
			{
                _logger.LogCritical($"GetCourseFilterAttributes() Error: {ex}");
                return StatusCode(500, "Internal Server Error");
			}
		}
    }
}