using VShop.DiscountApi.DTOs;

namespace VShop.DiscountApi.Repositories.Contracts;

public interface ICouponRepository
{
    Task<CouponDTO> GetCouponByCode(string couponCode);
}
