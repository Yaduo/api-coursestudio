using System.Collections.Generic;
using System.Threading.Tasks;
using CourseStudio.Application.Dtos.Trades;

namespace CourseStudio.Api.Services.Trades
{
    public interface ICouponService
    {
        Task<IList<CouponDto>> GetActivatedCouponsAsync();
        Task<CouponDto> GetActivatedCouponsByCodeAsync(string code);  
    }
}
