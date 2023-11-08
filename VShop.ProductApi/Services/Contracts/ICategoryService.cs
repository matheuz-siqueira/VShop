using VShop.ProductApi.DTOs;

namespace VShop.ProductApi.Services.Contracts;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetAll(); 
    Task<CategoryDTO> GetById(int id); 
    Task Create(CategoryDTO categoryDTO); 
    Task Update(CategoryDTO categoryDTO);
    Task Delete(int id);
}
