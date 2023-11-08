namespace VShop.ProductApi.Models;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public long Stock { get; set; }
    public string ImageUrl { get; set; }

    public Category Category { get; set; }
    public int CategoryId { get; set; }
}
