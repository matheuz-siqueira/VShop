using Microsoft.AspNetCore.Mvc;
using VShop.ProductApi.DTOs;
using VShop.ProductApi.Services.Contracts;

namespace VShop.ProductApi.Controllers;

[ApiController]
[Route("api/categories")]
[Produces("application/json")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;
    public CategoryController(ICategoryService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service)); 
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
    {
        var response = await _service.GetAll(); 
        if(!response.Any())
        {
            return NoContent(); 
        }
        return Ok(response); 
    }

    [HttpGet("{id}", Name = "GetCategory")]
    public async Task<ActionResult<CategoryDTO>> GetAll(int id)
    {
        var response = await _service.GetById(id);  
        if(response is null)
        {
            return NotFound(new {message = "Category not found"}); 
        }
        return Ok(response); 
        
    }

    [HttpPost]
    public async Task<ActionResult> Create(CategoryDTO categoryDTO)
    {
        if(categoryDTO is null)
            return BadRequest("invalid request");
        
        await _service.Create(categoryDTO);

        return new CreatedAtRouteResult("GetCategory", new { id = categoryDTO.Id}, 
            categoryDTO); 
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CategoryDTO categoryDTO)
    {
        if(id != categoryDTO.Id)
            return BadRequest(new { message = "different ids"});
        
        if(categoryDTO is null)
            return BadRequest(new { message = "Invalid request"});

        await _service.Update(categoryDTO);
        return NoContent();
    }
    

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var category = await _service.GetById(id);
        if(category is null)
            return NotFound(new { message = "Category not found"}) ;

        await _service.Delete(id); 
        return NoContent();
    }
}
