using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _service;
    private readonly ICartService _cartService;

    public HomeController(ILogger<HomeController> logger, IProductService service, 
        ICartService cartService)
    {
        _logger = logger;
        _service = service;
        _cartService = cartService;  
    }

    public async Task<IActionResult> Index()
    {
        var products = await _service.GetAll(""); 
        if(products is null)
            return View("Error"); 

        return View(products);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ProductViewModel>> ProductDetails(int id) 
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var product = await _service.GetById(id, token); 
        if(product is null)
            return View("Error"); 
        
        return View(product);
    }

    [HttpPost]
    [ActionName("ProductDetails")]
    [Authorize]
    public async Task<ActionResult<ProductViewModel>> ProductDetailsPost(
            ProductViewModel productVM)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        CartViewModel cart = new()
        {
            CartHeader = new()
            {
                UserId = User.Claims.Where(u => u.Type == "sub").FirstOrDefault().Value,
            }
        };

        CartItemViewModel cartItem = new()
        {
            Quantity = productVM.Quantity,
            ProductId = productVM.Id, 
            Product = await _service.GetById(productVM.Id, token)
        };

        List<CartItemViewModel> cartItemsVM = new()
        {
            cartItem
        };
        cart.CartItems = cartItemsVM;

        var result = await _cartService.AddItemToCartAsync(cart, token);
        
        if(result is not null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(productVM);

    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    [Authorize]
    public async Task<IActionResult> Login()
    {
        var access_token = await HttpContext.GetTokenAsync("access_token");
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }

}
