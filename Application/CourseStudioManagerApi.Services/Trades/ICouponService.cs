using System;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Trades;
using CourseStudio.Application.Dtos.Pagination;

namespace CourseStudioManager.Api.Services.Trades
{
    public interface ICouponService
    {
		Task<PaginationDto<CouponDto>> GetPagedCouponsAsync(string keywords, DateTime? createdFromUTC, DateTime? createdToUTC, int pageNumber, int pageSize);
		Task<CouponDto> GetCouponAsync(string couponCode);
		Task<CouponDto> CreateCouponAsync(CreateCouponRequestDto dto);
		Task DeleteCouponAsync(string couponCode);
    }
}
