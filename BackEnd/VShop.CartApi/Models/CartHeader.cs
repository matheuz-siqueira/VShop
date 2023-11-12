namespace VShop.CartApi.Models;

public class CartHeader
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string CuponCode { get; set; } = string.Empty;
}
