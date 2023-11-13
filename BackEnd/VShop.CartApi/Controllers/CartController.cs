using Microsoft.AspNetCore.Mvc;
using VShop.CartApi.DTOs;
using VShop.CartApi.Repositories.Contracts;

namespace VShop.CartApi.Controllers;

[ApiController]
[Route("api/cart")]
[Produces("application/json")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _repository;
    public CartController(ICartRepository repository)
    {
        _repository = repository;
    }    

    [HttpGet("getcart/{userId}")]
    public async Task<ActionResult<CartDTO>> GetByUserId(string userId)
    {
        var cart = await _repository.GetCartByUserIdAsync(userId); 
        if(cart is null)
            return NotFound(); 

        return Ok(cart); 
    }

    [HttpPost("addcart")]
    public async Task<ActionResult<CartDTO>> AddCart(CartDTO cartDTO)
    {
        var cart = await _repository.UpdateCartAsync(cartDTO); 
        if(cart is null)
            return NotFound(); 

        return Ok(cart); 
    }

    [HttpPost("applycoupon")]
    public async Task<ActionResult<CartDTO>> ApplyCoupon(CartDTO cartDTO)
    {
        var result = await _repository.ApplyCouponAsync(
            cartDTO.CartHeader.UserId, cartDTO.CartHeader.CouponCode);
        if(!result)
        {
            return NotFound(new 
                { message = $"CartHeader not found for userId = {cartDTO.CartHeader.UserId}"});
        }
        return Ok(result);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutHeaderDTO>> Checkout(CheckoutHeaderDTO checkout)
    {
        var cart = await _repository.GetCartByUserIdAsync(checkout.UserId);
        if(cart is null)
        {
            return NotFound(new { message = $"Cart not found for {checkout.UserId}"});
        }
        checkout.CartItems = cart.CartItems;
        checkout.DateTime = DateTime.Now;

        return Ok(checkout);

    }

    [HttpPut("updatecart")]
    public async Task<ActionResult<CartDTO>> UpdateCart(CartDTO cartDTO)
    {
        var cart = await _repository.UpdateCartAsync(cartDTO); 
        if(cart is null)
            return NotFound(); 

        return Ok(cart); 
    }

    [HttpDelete("deletecart/{id}")]
    public async Task<ActionResult<bool>> DeleteCart(int id)
    {
        var status = await _repository.DeleteItemCartAsync(id); 
        if(!status) 
            return BadRequest();

        return Ok(status);
    }

    [HttpDelete("deletecoupon/{userId}")]
    public async Task<ActionResult<CartDTO>> DeleteCoupon(string userId)
    {
        var result = await _repository.DeleteCouponAsync(userId);
        if(!result)
        {
            return NotFound(new 
                { message = $"Discount Coupon not found for userId = {userId}" });
        }
        return Ok(result);
    }
}
