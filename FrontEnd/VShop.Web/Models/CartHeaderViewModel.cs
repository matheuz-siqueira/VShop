namespace VShop.Web.Models;

public class CartHeaderViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; } 
    public string CuponCode { get; set; }
    public double TotalAmount { get; set; } = 0.00d;
}