using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using MediatR;
using AutoMapper;
using CourseStudio.Application.Common;
using CourseStudio.Application.Dtos.CourseAttributes;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Doamin.Models.CourseAttributes;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.CourseAttributes;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Exceptions;

namespace CourseStudioManager.Api.Services.CourseAttributes
{
	public class CourseAttributeService: BaseService, ICourseAttributeService
    {
		private readonly IEntityAttributeTypeRepository _entityAttributeTypeRepository;
		private readonly IEntityAttributeRepository _entityAttributeRepository;
        
        public CourseAttributeService
		(
			IEntityAttributeTypeRepository entityAttributeTypeRepository,
			IEntityAttributeRepository entityAttributeRepository,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_entityAttributeTypeRepository = entityAttributeTypeRepository;
			_entityAttributeRepository = entityAttributeRepository;
        }

		// TODO: do not copy yourself!!
		// TODO: there is a same method (but not the same name) in API CourseAttributeServices
		public async Task<IList<EntityAttributeTypeDto>> GetCourseAttributesAsync(IList<int?> parentAttributeIds) 
		{
			var entityAttributeTypes = await _entityAttributeTypeRepository.GetEntityAttributeTypesAsync();
            var entityAttributes = await _entityAttributeRepository.GetEntityAttributeByParentIdsAsync(parentAttributeIds);
            // TODO: dont understand why it mapping drieactly.
            return Mapper.Map<IList<EntityAttributeTypeDto>>(entityAttributeTypes);
		}
        
		public async Task<IList<EntityAttributeTypeDto>> GetEntityAttributeTypesAsync() 
		{
			var types = await _entityAttributeTypeRepository.GetEntityAttributeTypesAsync();
			if (!types.Any())
            {
                throw new NotFoundException();
            }
			return Mapper.Map<IList<EntityAttributeTypeDto>>(types);
		}

		public async Task<PaginationDto<EntityAttributeDto>> GetPagedEntityAttributesByTypeAsync(int entityAttributeTypeId, int pageNumber, int pageSize)
        {
			var attributes = await _entityAttributeRepository.GetPagedEntityAttributesByTypeAsync(entityAttributeTypeId, pageNumber, pageSize);
			return Mapper.Map<PaginationDto<EntityAttributeDto>>(attributes);
        }

		public async Task CreateEntityAttributeTypeAsync(EntityAttributeTypeDto dto) 
		{
            throw new Exception();
		}

		public async Task UpdateEntityAttributeTypeAsync(int entityAttributeTypeId, EntityAttributeTypeDto newType) 
		{
			var entityAttributeType = await _entityAttributeTypeRepository.GetEntityAttributeTypeByIdAsync(entityAttributeTypeId);
			if (entityAttributeType == null)
			{
				throw new NotFoundException();
			}

			entityAttributeType.Name = newType.Name;
			await _entityAttributeTypeRepository.SaveAsync();
		}

		public async Task DeleteEntityAttributeTypeAsync(int entityAttributeTypeId)
		{
			var entityAttributeType = await _entityAttributeTypeRepository.GetEntityAttributeTypeByIdAsync(entityAttributeTypeId);
			if (entityAttributeType == null)
            {
                throw new NotFoundException();
            }

			// Cannot remove if the entityAttributeType is being used
			if (entityAttributeType.EntityAttributes.Any())
			{
				throw new BadRequestException("EntityAttributes " + String.Join(", ", entityAttributeType.EntityAttributes.Select(e => e.Id).ToList())+ " is being used. ");
			}

			_entityAttributeTypeRepository.Remove(entityAttributeType);
			await _entityAttributeTypeRepository.SaveAsync();
		}

		public async Task CreateEntityAttributeAsync(int entityAttributeTypeId, EntityAttributeDto entityAttributeDto) 
		{
            throw new Exception();
		}

		public async Task UpdateEntityAttributeAsync(int entityAttributeTypeId, int entityAttributeId, EntityAttributeDto entityAttributeDto) 
		{
            //if (entityAttributeDto.EntityAttributeTypeId == null)
            //         {
            //             throw new BadRequestException("EntityAttributeTypeId cannot be null");
            //         }

            //var entityAttributeType = await _entityAttributeTypeRepository.GetEntityAttributeTypeByIdAsync(entityAttributeTypeId);
            //         if (entityAttributeType == null)
            //         {
            //	throw new NotFoundException("EntityAttributeType not found.");
            //         }

            //var entityAttribute = entityAttributeType.EntityAttributes.SingleOrDefault(e => e.Id == entityAttributeId);
            //if (entityAttribute == null) 
            //{
            //	throw new NotFoundException("EntityAttribute not found.");
            //}

            //entityAttribute.Update(entityAttributeDto.EntityAttributeTypeId.Value, entityAttributeDto.ParentId, entityAttributeDto.Value, entityAttributeDto.IsSearchable);
            //await _entityAttributeRepository.SaveAsync();
            throw new Exception();
		}

		public async Task DeleteEntityAttributeAsync(int entityAttributeTypeId, int entityAttributeId)
		{
			var entityAttributeType = await _entityAttributeTypeRepository.GetEntityAttributeTypeByIdAsync(entityAttributeTypeId);
            if (entityAttributeType == null)
            {
                throw new NotFoundException("EntityAttributeType not found.");
            }

            var entityAttribute = entityAttributeType.EntityAttributes.SingleOrDefault(e => e.Id == entityAttributeId);
            if (entityAttribute == null)
            {
                throw new NotFoundException("EntityAttribute not found.");
            }

			_entityAttributeRepository.Remove(entityAttribute);
			await _entityAttributeRepository.SaveAsync();
		}
    }
}
