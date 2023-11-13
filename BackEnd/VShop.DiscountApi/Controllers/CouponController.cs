using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VShop.DiscountApi.DTOs;
using VShop.DiscountApi.Repositories.Contracts;

namespace VShop.DiscountApi.Controllers;

[ApiController]
[Route("api/coupon")]
[Produces("application/json")]
public class CouponController : ControllerBase
{
    private readonly ICouponRepository _repository;
    public CouponController(ICouponRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{couponCode}")]
    [Authorize]
    public async Task<ActionResult<CouponDTO>> GetDiscountByCouponCode(string couponCode)
    {
        var coupon = await _repository.GetCouponByCode(couponCode); 
        if(coupon is null)
            return NotFound(new {message = $"Coupon Code {couponCode} not found"});

        return Ok(coupon);
    }


}
