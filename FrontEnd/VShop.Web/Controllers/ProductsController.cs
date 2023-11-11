using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VShop.Web.Models;
using VShop.Web.Roles;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Controllers;

[Authorize(Roles = Role.Admin)]
public class ProductsController : Controller
{
    private readonly IProductService _service;
    private readonly ICategoryService _categoryService;
    public ProductsController(IProductService service, 
        ICategoryService categoryService)
    {
        _service = service;
        _categoryService = categoryService;  
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
    {
        var result = await _service.GetAll(await GetToken());
        if(result is null)
            return View("Error");

        return View(result);
    }

    [HttpGet]
    public async Task<ActionResult> CreateProduct()
    {
        ViewBag.CategoryId = 
            new SelectList(await _categoryService.GetAll(await GetToken()), "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> CreateProduct(ProductViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await _service.Create(model, await GetToken()); 
            if(result is not null)
                return RedirectToAction(nameof(Index));
        }
        else 
        {
            ViewBag.CategoryId = new SelectList(await _categoryService.GetAll(await GetToken()), "Id", "Name");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> UpdateProduct(int id)
    {
        ViewBag.CategoryId = new SelectList(await _categoryService.GetAll(await GetToken()), "Id", "Name");
        var result = await _service.GetById(id, await GetToken()); 

        if(result is null)
            return View("Error"); 

        return View(result);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateProduct(ProductViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await _service.Update(model, await GetToken());
            if(result is not null)
                return RedirectToAction(nameof(Index)); 
        }
        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult<ProductViewModel>> DeleteProduct(int id)
    {
        var result = await _service.GetById(id, await GetToken());
        if(result is null)
            return View("Error"); 

        return View(result);
    }

    [HttpPost(), ActionName("DeleteProduct")]
    public async Task<ActionResult> DeteteConfirmed(int id)
    {
        var result = await _service.Delete(id, await GetToken());
        if(!result)
            return View("Error"); 

        return RedirectToAction("Index");
    }

    private async Task<string> GetToken()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        return token;
    }   

}
