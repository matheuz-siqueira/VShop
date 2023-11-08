using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VShop.ProductApi.Migrations
{
    public partial class SeedProducts : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Products(Name, Description, Price, Stock, ImageUrl, CategoryId)" + 
            "Values('caderno', 'caderno aspiral', 755, 10, 'cadeno1.jpg', 1)");

            mb.Sql("Insert into Products(Name, Description, Price, Stock, ImageUrl, CategoryId)" + 
            "Values('lápis', 'lapis faber castel', 4.55, 23, 'lapis1.jpg', 1)");

            mb.Sql("Insert into Products(Name, Description, Price, Stock, ImageUrl, CategoryId)" + 
            "Values('clips','Clips para papel', 3.30, 67, 'clips1.jpg', 2)");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("delete from Products");
        }
    }
}
