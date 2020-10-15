using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using CourseStudio.Application.Common;
using CourseStudio.Application.Dtos.CourseAttributes;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Doamin.Models.CourseAttributes;
using CourseStudio.Domain.Repositories.CourseAttributes;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Api.Services.CourseAttributes
{
	public class CourseAttributeServices : BaseService, ICourseAttributeServices
    {
		private readonly IEntityAttributeTypeRepository _entityAttributeTypeRepository;
		private readonly IEntityAttributeRepository _entityAttributeRepository;

        public CourseAttributeServices(
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

		public async Task<IList<EntityAttributeTypeDto>> GetEntityAttributeTypeAsync(IList<int?> parentAttributeIds)
        {
			var entityAttributeTypes = await _entityAttributeTypeRepository.GetEntityAttributeTypesAsync();
			var entityAttributes = await _entityAttributeRepository.GetEntityAttributeByParentIdsAsync(parentAttributeIds); 
			// TODO: dont understand why it mapping drieactly.
			return Mapper.Map<IList<EntityAttributeTypeDto>>(entityAttributeTypes);
        }
    }
}
