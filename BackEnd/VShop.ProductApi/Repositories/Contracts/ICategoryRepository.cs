using VShop.ProductApi.Models;

namespace VShop.ProductApi.Repositories.Contracts;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAll();
    Task<Category> GetById(int id); 
    Task<Category> Create(Category category); 
    Task<Category> Update(Category category);
    Task<Category> Delete(int id);
}
