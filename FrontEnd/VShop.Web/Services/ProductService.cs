using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string apiEndpoint = "api/products/";
    private readonly JsonSerializerOptions _options;
    private ProductViewModel productVM; 
    private IEnumerable<ProductViewModel> productsVM;
    public ProductService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory; 
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
    public async Task<IEnumerable<ProductViewModel>> GetAll(string token)
    {
        var client = _clientFactory.CreateClient("ProductApi"); 
        PutTokenInHeaderAuthorization(token, client);
        using (var response = await client.GetAsync(apiEndpoint))
        {
            if(response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync(); 
                productsVM = await JsonSerializer
                    .DeserializeAsync<IEnumerable<ProductViewModel>>(apiResponse, _options);
            }
            else 
            {
                return null; 
            }
        }
        return productsVM;
    }

    public async Task<ProductViewModel> GetById(int id, string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthorization(token, client); 
        using (var response = await client.GetAsync(apiEndpoint + id))
        {
            if(response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productVM = await JsonSerializer
                    .DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else 
            {
                return null;
            }
        }
        return productVM;
    }
    public async Task<ProductViewModel> Create(ProductViewModel model, string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthorization(token, client);
        StringContent content = new (JsonSerializer.Serialize(model), 
            Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(apiEndpoint, content))
        {
            if(response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                model = await JsonSerializer
                    .DeserializeAsync<ProductViewModel>(apiResponse, _options); 
            }
            else
            {
                return null; 
            }
        }
        return model; 
    }
    public async Task<ProductViewModel> Update(ProductViewModel model, string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthorization(token, client); 
        ProductViewModel productUpdated = new();

        using var response = await client.PutAsJsonAsync(apiEndpoint, model);
        if(response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            productUpdated = await JsonSerializer
                .DeserializeAsync<ProductViewModel>(apiResponse, _options); 
        }
        else 
        {
            return null; 
        }
        
        return productUpdated;
    }
    public async Task<bool> Delete(int id, string token)
    {
        var client = _clientFactory.CreateClient("ProductApi");
        PutTokenInHeaderAuthorization(token, client); 
        using var response = await client.DeleteAsync(apiEndpoint + id);
        if(response.IsSuccessStatusCode)
        {
            return true;  
        }
        return false; 
    }

    private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }
}
