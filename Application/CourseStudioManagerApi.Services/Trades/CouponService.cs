using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using MediatR;
using AutoMapper;
using CourseStudio.Application.Common;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Application.Dtos.Pagination;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Repositories.Users;
using CourseStudio.Domain.Repositories.Trades;
using CourseStudio.Lib.Exceptions;

namespace CourseStudioManager.Api.Services.Trades
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

		public async Task<PaginationDto<CouponDto>> GetPagedCouponsAsync(string keywords, DateTime? createdFromUTC, DateTime? createdToUTC, int pageNumber, int pageSize)
		{
			var coupons = await _couponRepository.GetPagedCouponsAsync(keywords, createdFromUTC, createdToUTC, pageNumber, pageSize);
			return Mapper.Map<PaginationDto<CouponDto>>(coupons);
		}

		public async Task<CouponDto> GetCouponAsync(string couponCode)
		{
			var coupon = await _couponRepository.GetCouponByCodeAsync(couponCode);
			if(coupon == null) {
				throw new NotFoundException("coupon not found");
			}
			return Mapper.Map<CouponDto>(coupon);
        }

		public async Task<CouponDto> CreateCouponAsync(CreateCouponRequestDto dto) 
		{
            throw new Exception();
		}

		public async Task DeleteCouponAsync(string couponCode) 
		{
            throw new Exception();
        }
    }
}
