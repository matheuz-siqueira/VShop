using VShop.ProductApi.DTOs;

namespace VShop.ProductApi.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetAll(); 
    Task<ProductDTO> GetById(int id); 
    Task Create(ProductDTO productDTO); 
    Task Update(ProductDTO productDTO);
    Task Delete(int id);    
}
