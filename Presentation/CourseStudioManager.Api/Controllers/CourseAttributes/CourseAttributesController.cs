using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseStudio.Application.Dtos.CourseAttributes;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Exceptions;
using CourseStudio.Presentation.Common;
using CourseStudio.Presentation.Common.Filters;
using CourseStudio.Presentation.Common.ModelBinders;
using CourseStudioManager.Api.Services.CourseAttributes;

namespace CourseStudioManager.Api.Controllers.CourseAttributes
{
	[Route("api/courseAttributes")]
	public class CourseAttributesController: BaseController
    {
		private readonly ICourseAttributeService _courseAttributeService;
        
		public CourseAttributesController
        (
			ICourseAttributeService courseAttributeService,
            ILogger<CourseAttributesController> logger,
            IUrlHelper urlHelper
        ) : base(logger, urlHelper)
        {
			_courseAttributeService = courseAttributeService;
        }

		// GET api/courseAttributes
        [HttpGet]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> GetCourseAttributes([ModelBinder(BinderType = typeof(ArrayModelBinder))] IList<int?> parentAttributeIds)
        {
            try
            {
				var courseAttributes = await _courseAttributeService.GetCourseAttributesAsync(parentAttributeIds);
				return Ok(courseAttributes);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetCourseAttributes() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		// GET api/courseAttributes/entityAttributeTypes
		[HttpGet("entityAttributeTypes")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> GetEntityAttributeTypes()
        {
            try
            {
				var entityAttributeTypes = await _courseAttributeService.GetEntityAttributeTypesAsync();
				if (entityAttributeTypes == null) 
				{
					return NotFound();
				}

				return Ok(entityAttributeTypes);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetEntityAttributeTypes() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
        
		// POST api/courseAttributes/entityAttributeTypes
		[HttpPost("entityAttributeTypes")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> CreateEntityAttributeType([FromBody] EntityAttributeTypeDto dto)
        {
            try
            {
				await _courseAttributeService.CreateEntityAttributeTypeAsync(dto);
				return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"CreateEntityAttributeType() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		// PUT api/courseAttributes/entityAttributeTypes/6
		[HttpPut("entityAttributeTypes/{entityAttributeTypeId}")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> UpdateEntityAttributeType(int entityAttributeTypeId, [FromBody] EntityAttributeTypeDto dto)
        {
            try
            {
				await _courseAttributeService.UpdateEntityAttributeTypeAsync(entityAttributeTypeId, dto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"UpdateEntityAttributeType() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
      
		// DELETE api/courseAttributes/entityAttributeTypes/6
		[HttpDelete("entityAttributeTypes/{entityAttributeTypeId}")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        public async Task<IActionResult> DeleteEntityAttributeType(int entityAttributeTypeId)
        {
            try
            {
				await _courseAttributeService.DeleteEntityAttributeTypeAsync(entityAttributeTypeId);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"DeleteEntityAttributeType() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		// GET api/courseAttributes/entityAttributeTypes/2/entityAttributes
		[HttpGet("entityAttributeTypes/{entityAttributeTypeId}/entityAttributes")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		public async Task<IActionResult> GetEntityAttributesByType(int entityAttributeTypeId, PaginationParameters pagingParameters)
        {
            try
            {
				if (pagingParameters.PageNumber <= 0)
	            {
	                return BadRequest("page number must larger then 0");
	            }

				var results = await _courseAttributeService.GetPagedEntityAttributesByTypeAsync(entityAttributeTypeId, pagingParameters.PageNumber, pagingParameters.PageSize);
				if (!results.Items.Any())
				{
				    return NotFound();
				}

				var paginationMetadata = GeneratePaginationMetadata(results.TotalCount, results.TotalPages, results.PageSize, results.CurrentPage);
				Response.Headers.Add("X-Pagination", paginationMetadata);
				Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
            
				return Ok(results.Items);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"GetEntityAttributesByType() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
    
		// POST api/courseAttributes/entityAttributeTypes/2/entityAttributes
		[HttpPost("entityAttributeTypes/{entityAttributeTypeId}/entityAttributes")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		[ValidateModel]
		public async Task<IActionResult> CreateEntityAttribute(int entityAttributeTypeId, [FromBody] EntityAttributeDto entityAttributeDto)
        {
            try
            {
				await _courseAttributeService.CreateEntityAttributeAsync(entityAttributeTypeId, entityAttributeDto);
				return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"CreateEntityAttribute() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		// PUT api/courseAttributes/entityAttributeTypes/2/entityAttributes/7
		[HttpPut("entityAttributeTypes/{entityAttributeTypeId}/entityAttributes/{entityAttributeId}")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
		[ValidateModel]
		public async Task<IActionResult> UpdateEntityAttribute(int entityAttributeTypeId, int entityAttributeId, [FromBody] EntityAttributeDto entityAttributeDto)
        {
            try
            {
				await _courseAttributeService.UpdateEntityAttributeAsync(entityAttributeTypeId, entityAttributeId, entityAttributeDto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"UpdateEntityAttribute() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

		// DELETE api/courseAttributes/entityAttributeTypes/2/entityAttributes/7
		[HttpDelete("entityAttributeTypes/{entityAttributeTypeId}/entityAttributes/{entityAttributeId}")]
        [Authorize(Roles = ApplicationPolicies.DefaultRoles.Staff)]
        public async Task<IActionResult> DeleteEntityAttribute(int entityAttributeTypeId, int entityAttributeId)
        {
            try
            {
                await _courseAttributeService.DeleteEntityAttributeAsync(entityAttributeTypeId, entityAttributeId);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"DeleteEntityAttribute() error {ex}");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }
	}
}
