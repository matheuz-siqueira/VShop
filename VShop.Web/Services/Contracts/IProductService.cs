using VShop.Web.Models;

namespace VShop.Web.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>> GetAll();
    Task<ProductViewModel> GetById(int id); 
    Task<ProductViewModel> Create(ProductViewModel model); 
    Task<ProductViewModel> Update(ProductViewModel model);
    Task<bool> Delete(int id);
}
