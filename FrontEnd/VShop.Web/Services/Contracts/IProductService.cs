using VShop.Web.Models;

namespace VShop.Web.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>> GetAll(string token);
    Task<ProductViewModel> GetById(int id, string token); 
    Task<ProductViewModel> Create(ProductViewModel model, string token); 
    Task<ProductViewModel> Update(ProductViewModel model, string token);
    Task<bool> Delete(int id, string token);
}
