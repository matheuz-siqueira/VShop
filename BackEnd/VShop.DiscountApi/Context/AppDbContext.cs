using Microsoft.EntityFrameworkCore;
using VShop.DiscountApi.Models;

namespace VShop.DiscountApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
    { }

    public DbSet<Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Coupon>().HasKey(c => c.Id); 
        mb.Entity<Coupon>().Property(c => c.CouponCode).HasMaxLength(30).IsRequired();
        mb.Entity<Coupon>().Property(c => c.Discount).HasPrecision(10, 2).IsRequired(); 

        mb.Entity<Coupon>().HasData(new Coupon
        {
            Id = 1, 
            CouponCode = "SHOP_PROMO_10",
            Discount = 10
        });
        mb.Entity<Coupon>().HasData(new Coupon
        {
            Id = 2, 
            CouponCode = "SHOP_PROMO_20",
            Discount = 20
        });
    }
}
