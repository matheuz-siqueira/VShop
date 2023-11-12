namespace VShop.DiscountApi.DTOs;

public class CouponDTO
{
    public int Id { get; set; }
    public string CouponCode { get; set; }
    public decimal Discount { get; set; }
}
