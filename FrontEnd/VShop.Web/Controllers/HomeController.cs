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

    public HomeController(ILogger<HomeController> logger, IProductService service)
    {
        _logger = logger;
        _service = service; 
    }

    public async Task<IActionResult> Index()
    {
        var products = await _service.GetAll(""); 
        if(products is null)
            return View("Error"); 

        return View(products);
    }

    [HttpGet]
    public async Task<ActionResult<ProductViewModel>> ProductDetails(int id) 
    {
        var product = await _service.GetById(id, string.Empty); 
        if(product is null)
            return View("Error"); 
        
        return View(product);
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
