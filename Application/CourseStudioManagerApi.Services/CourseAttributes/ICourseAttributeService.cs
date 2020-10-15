using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.CourseAttributes;
using CourseStudio.Application.Dtos.Pagination;

namespace CourseStudioManager.Api.Services.CourseAttributes
{
    public interface ICourseAttributeService
    {
		Task<IList<EntityAttributeTypeDto>> GetCourseAttributesAsync(IList<int?> parentAttributeIds);
		Task<IList<EntityAttributeTypeDto>> GetEntityAttributeTypesAsync();
		Task<PaginationDto<EntityAttributeDto>> GetPagedEntityAttributesByTypeAsync(int entityAttributeTypeId, int pageNumber, int pageSize);
		Task CreateEntityAttributeTypeAsync(EntityAttributeTypeDto dto);
		Task UpdateEntityAttributeTypeAsync(int entityAttributeTypeId, EntityAttributeTypeDto newType);
		Task DeleteEntityAttributeTypeAsync(int entityAttributeTypeId);
		Task CreateEntityAttributeAsync(int entityAttributeTypeId, EntityAttributeDto entityAttributeDto);
		Task UpdateEntityAttributeAsync(int entityAttributeTypeId, int entityAttributeId, EntityAttributeDto entityAttributeDto);
		Task DeleteEntityAttributeAsync(int entityAttributeTypeId, int entityAttributeId);
    }
}