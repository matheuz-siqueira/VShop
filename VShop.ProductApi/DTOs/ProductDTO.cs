using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VShop.ProductApi.Models;

namespace VShop.ProductApi.DTOs;

public class ProductDTO
{

    [Required]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "the name is required")]
    [MinLength(3)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required(ErrorMessage = "description is required")]
    [MinLength(5)]
    [MaxLength(250)] 
    public string Description { get; set; }
    
    [Required(ErrorMessage = "the price is required")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "the stock is required")]
    [Range(1, 9999)]
    public long Stock { get; set; }
    public string ImageUrl { get; set; }

    public string CategoryName { get; set; }
    public int CategoryId { get; set; }
}
