using VShop.CartApi.DTOs;

namespace VShop.CartApi.Repositories.Contracts;

public interface ICartRepository
{
    Task<CartDTO> GetCartByUserIdAsync(string userId); 
    Task<CartDTO> UpdateCartAsync(CartDTO cartDTO);
    Task<bool> CleanCartAsync(string userId); 
    Task<bool> DeleteItemCartAsync(int cartItemId);
    Task<bool> ApplyCouponAsync(string userId, string couponCode);
    Task<bool> DeleteCouponAsync(string userId);

}
