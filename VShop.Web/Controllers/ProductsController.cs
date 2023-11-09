using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Controllers;

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

    public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
    {
        var result = await _service.GetAll();
        if(result is null)
            return View("Error");

        return View(result);
    }

    [HttpGet]
    public async Task<ActionResult> CreateProduct()
    {
        ViewBag.CategoryId = 
            new SelectList(await _categoryService.GetAll(), "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> CreateProduct(ProductViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await _service.Create(model); 
            if(result is not null)
                return RedirectToAction(nameof(Index));
        }
        else 
        {
            ViewBag.CategoryId = new SelectList(await _categoryService.GetAll(), "Id", "Name");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> UpdateProduct(int id)
    {
        ViewBag.CategoryId = new SelectList(await _categoryService.GetAll(), "Id", "Name");
        var result = await _service.GetById(id); 

        if(result is null)
            return View("Error"); 

        return View(result);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateProduct(ProductViewModel model)
    {
        if(ModelState.IsValid)
        {
            var result = await _service.Update(model);
            if(result is not null)
                return RedirectToAction(nameof(Index)); 
        }
        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult<ProductViewModel>> DeleteProduct(int id)
    {
        var result = await _service.GetById(id);
        if(result is null)
            return View("Error"); 

        return View(result);
    }

    [HttpPost(), ActionName("DeleteProduct")]
    public async Task<ActionResult> DeteteConfirmed(int id)
    {
        var result = await _service.Delete(id);
        if(!result)
            return View("Error"); 

        return RedirectToAction("Index");
    }

}
