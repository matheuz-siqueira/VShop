using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Contracts;

namespace VShop.Web.Services;

public class CartService : ICartService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _options; 
    private const string apiEndpoint = "api/cart"; 
    private CartViewModel CartVM = new ();

    public CartService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory; 
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    public async Task<CartViewModel> GetCartByUseIdAsync(string userId, string token)
    {
        var client = _clientFactory.CreateClient("CartApi");
        PutTokenInHeaderAuthorization(client, token);

        using var response = await client.GetAsync($"{apiEndpoint}/getcart/{userId}");
            if(response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync(); 
                CartVM = await JsonSerializer.DeserializeAsync<CartViewModel>
                                (apiResponse, _options); 
            }
            else 
            {
                return null;
            }
        return CartVM;
    }
    public async Task<CartViewModel> AddItemToCartAsync(CartViewModel cartVM, string token)
    {
        var client = _clientFactory.CreateClient("CartApi");
        PutTokenInHeaderAuthorization(client, token); 
        StringContent content = new StringContent(JsonSerializer.Serialize(cartVM), 
            Encoding.UTF8, "application/json"); 

        using var response = await client.PostAsync($"{apiEndpoint}/addcart", content);

        if(response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync(); 
            CartVM = await JsonSerializer.DeserializeAsync<CartViewModel>
                (apiResponse, _options); 
        }
        else 
        {
            return null;
        }
        return CartVM;

    }
    public async Task<CartViewModel> UpdateCartAsync(CartViewModel cartVM, string token)
    {
        var client = _clientFactory.CreateClient("CartApi"); 
        PutTokenInHeaderAuthorization(client, token); 
        CartViewModel updated = new();

        using var response = await client.PutAsJsonAsync($"{apiEndpoint}/updatecart", cartVM);
        if(response.IsSuccessStatusCode)
        {
            var apiReponse = await response.Content.ReadAsStreamAsync(); 
            updated = await JsonSerializer.DeserializeAsync<CartViewModel>
                (apiReponse, _options); 
        }
        else 
        {
            return null; 
        }
        return updated; 

    }
    public async Task<bool> RemoveItemFromCartAsync(int cartId, string token)
    {
        var client = _clientFactory.CreateClient("CartApi");
        PutTokenInHeaderAuthorization(client, token); 

        using var response = await client.DeleteAsync($"{apiEndpoint}/deletecart/{cartId}");
        if(response.IsSuccessStatusCode)
        {
            return true; 
        } 
        return false; 
    }
    public Task<bool> ApplyCouponAsync(CartViewModel cartVM, string couponCode, string token)
    {
        throw new NotImplementedException();
    }
    public Task<bool> RemoveCouponAsync(string userId, string token)
    {
        throw new NotImplementedException();
    }
    public Task<bool> ClearCartAsync(string userId, string token)
    {
        throw new NotImplementedException();
    }
    public Task<CartViewModel> CheckoutAsync(CartHeaderViewModel cartHeader, string token)
    {
        throw new NotImplementedException();
    }

    private static void PutTokenInHeaderAuthorization(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token); 
    }
}
