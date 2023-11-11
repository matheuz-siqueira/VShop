using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VShop.ProductApi.DTOs;
using VShop.ProductApi.Roles;
using VShop.ProductApi.Services.Contracts;

namespace VShop.ProductApi.Controllers;

[ApiController]
[Route("api/products")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    public ProductController(IProductService service)
    {
        _service = service; 
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
    {
        var response = await _service.GetAll(); 
        if(!response.Any())
        {
            return NoContent(); 
        }
        return Ok(response);
    }

    [HttpGet("{id}", Name = "GetProduct")]
    public async Task<ActionResult<ProductDTO>> GetById(int id)
    {
        var response = await _service.GetById(id);
        if(response is null)
            return NotFound(new {message = "Product not found"}); 

        return Ok(response); 
    }

    [HttpPost]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult> Create(ProductDTO productDTO)
    {
        if(productDTO is null)
            return BadRequest(new {message = "Invalid request"});
        
        await _service.Create(productDTO);

        return new CreatedAtRouteResult("GetProduct", new { id = productDTO.Id}, productDTO); 
    }

    [HttpPut]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult> Update(ProductDTO productDTO)
    {
        if(productDTO is null)
            return BadRequest(new { message = "Product not found"}); 

        await _service.Update(productDTO); 
        return Ok(productDTO); 
        
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult> Delete(int id)
    {
        var product = await _service.GetById(id); 
        if(product is null)
            return NotFound(new { message = "Product not found"}); 

        await _service.Delete(id); 
        return NoContent(); 
    }
}
