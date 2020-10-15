using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CourseStudio.Application.Common.Helpers;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace CourseStudio.Application.Common
{
	public class BaseService
    {

		protected readonly IHttpContextAccessor _httpContextAccessor;
		protected readonly IMediator _mediator;
		protected readonly IUserRepository _userRepository;
		protected readonly UserManager<ApplicationUser> _userManager;

		public BaseService(
			IMediator mediator,
			IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
			UserManager<ApplicationUser> userManager
		)
        {
			_mediator = mediator;
			_httpContextAccessor = httpContextAccessor;
			_userRepository = userRepository;
			_userManager = userManager;
        }

		public async Task<ApplicationUser> GetCurrentUser(bool includeActivated=true)
        {
			return await IdentityHelper.GetCurrentUser(_httpContextAccessor, _userRepository, _userManager, includeActivated);
        }
    }
}
