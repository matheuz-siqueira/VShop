using AutoMapper;
using VShop.ProductApi.DTOs;
using VShop.ProductApi.Models;
using VShop.ProductApi.Repositories.Contracts;
using VShop.ProductApi.Services.Contracts;

namespace VShop.ProductApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper; 
    }
    public async Task<IEnumerable<ProductDTO>> GetAll()
    {
        var products = await _repository.GetAll();
        return _mapper.Map<IEnumerable<ProductDTO>>(products);        
    }
    public async Task<ProductDTO> GetById(int id)
    {
        var product = await _repository.GetById(id); 
        return _mapper.Map<ProductDTO>(product); 
    }
    public async Task Create(ProductDTO productDTO)
    {
        var entity = _mapper.Map<Product>(productDTO); 
        await _repository.Create(entity); 
        productDTO.Id = entity.Id; 
    }
    public async Task Update(ProductDTO productDTO)
    {
        var entity = _mapper.Map<Product>(productDTO); 
        await _repository.Update(entity); 
    }
    public async Task Delete(int id)
    {
        var entity = await _repository.GetById(id);
        await _repository.Delete(entity.Id);     
    }

}
