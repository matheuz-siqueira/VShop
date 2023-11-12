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
}
