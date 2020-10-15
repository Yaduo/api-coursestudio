using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MediatR;
using AutoMapper;
using CourseStudio.Application.Common;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Domain.Repositories.Trades;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Doamin.Models.Users;

namespace CourseStudio.Api.Services.Trades
{
	public class CouponService : BaseService, ICouponService
    {
		private readonly ICouponRepository _couponRepository;

        public CouponService(
			ICouponRepository couponRepository,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
			IUserRepository userRepository,
            UserManager<ApplicationUser> userManager
		) : base(mediator, httpContextAccessor, userRepository, userManager)
        {
			_couponRepository = couponRepository;
        }

        public async Task<IList<CouponDto>> GetActivatedCouponsAsync()
        {
            return Mapper.Map<IList<CouponDto>>(await _couponRepository.GetActivatedCouponsAsync());
        }

        public async Task<CouponDto> GetActivatedCouponsByCodeAsync(string code)
        {
            return Mapper.Map<CouponDto>(await _couponRepository.GetCouponByCodeAsync(code));
        }
    }
}
