using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _service;
    private readonly ICouponService _couponService;
    public CartController(ICartService service, ICouponService couponService)
    {
        _service = service;
        _couponService = couponService; 
    }    

    [Authorize]
    public async Task<IActionResult> Index()
    {
        CartViewModel cartVM = await GetCartByUser();
        if(cartVM is null)
        {
            ModelState.AddModelError("CartNotFound", "Does not exist a cart yet, Come on Shopping");
            return View("Views/Cart/CartNotFound.cshtml");
        }
        
        return View(cartVM);
    }

    public async Task<IActionResult> RemoveItem(int id)
    {
        var result = await _service.RemoveItemFromCartAsync(id, await GetAccessToken());
        if(result)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(id);
    }

    private async Task<CartViewModel> GetCartByUser()
    {
        var cart = await _service.GetCartByUseIdAsync(GetUserId(), await GetAccessToken());
        if(cart?.CartHeader is not null)
        {

            if(!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            {
                var coupon = await _couponService.GetDiscountCoupon(cart.CartHeader.CouponCode, await GetAccessToken());
            
                if(coupon?.CouponCode is not null)
                {
                    cart.CartHeader.Discount = coupon.Discount;
                }
            }

            foreach(var item in cart.CartItems)
            {
                cart.CartHeader.TotalAmount += item.Product.Price * item.Quantity; 
            }

            cart.CartHeader.TotalAmount = cart.CartHeader.TotalAmount -
                (cart.CartHeader.TotalAmount * cart.CartHeader.Discount) / 100;
        }
        return cart;
    }   

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        CartViewModel cartVM = await GetCartByUser(); 
        return View(cartVM);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CartViewModel cartVM)
    {
        if(ModelState.IsValid)
        {
            var result = await _service.CheckoutAsync(cartVM.CartHeader, await GetAccessToken());
            if(result is not null)
            {
                return RedirectToAction(nameof(CheckoutCompleted));
            }
        }
        return View(cartVM);
    }

    [HttpGet]
    public IActionResult CheckoutCompleted()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(CartViewModel cart)
    {
        if(ModelState.IsValid)
        {
            var result = await _service.ApplyCouponAsync(cart, await GetAccessToken()); 
            if(result)
            {
                return RedirectToAction(nameof(Index)); 
            }
        }
        return View();
    } 

    [HttpPost]
    public async Task<IActionResult> DeleteCoupon()
    {
        var result = await _service.RemoveCouponAsync(GetUserId(), await GetAccessToken());
        if(result)
        {
            return RedirectToAction(nameof(Index)); 
        }
        return View();
    }

    private async Task<string> GetAccessToken()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }

    private string GetUserId()
    {
        return User.Claims.Where(u => u.Type == "sub").FirstOrDefault().Value;
    }
}
