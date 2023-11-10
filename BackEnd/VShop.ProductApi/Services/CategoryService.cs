using AutoMapper;
using VShop.ProductApi.DTOs;
using VShop.ProductApi.Models;
using VShop.ProductApi.Repositories.Contracts;
using VShop.ProductApi.Services.Contracts;

namespace VShop.ProductApi.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    public CategoryService(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository; 
        _mapper = mapper; 
    }

    public async Task<IEnumerable<CategoryDTO>> GetAll()
    {
        var categories = await _repository.GetAll();
        return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
    }

    public async Task<CategoryDTO> GetById(int id)
    {
        var category = await _repository.GetById(id); 
        return _mapper.Map<CategoryDTO>(category); 
    }
    public async Task Create(CategoryDTO categoryDTO)
    {
        var entity = _mapper.Map<Category>(categoryDTO);
        await _repository.Create(entity);
        categoryDTO.Id = entity.Id; 
    }
    public async Task Update(CategoryDTO categoryDTO)
    {
        var entity = _mapper.Map<Category>(categoryDTO);
        await _repository.Update(entity); 
    }
    public async Task Delete(int id)
    {
        var entity = _repository.GetById(id);
        await _repository.Delete(entity.Id); 
    }
}
