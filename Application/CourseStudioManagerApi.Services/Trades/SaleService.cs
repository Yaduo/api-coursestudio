using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MediatR;
using CourseStudio.Application.Common;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Users;

namespace CourseStudioManager.Api.Services.Trades
{
	public class SaleService: BaseService, ISaleService
    {
		public SaleService(
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
        }

		public async Task GetPagedSalesAsync(DateTime? from, DateTime? to, int pageNumber, int pageSize) 
		{
			
		}
    }
}
