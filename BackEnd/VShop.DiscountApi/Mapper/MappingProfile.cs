using AutoMapper; 
using VShop.DiscountApi.Models; 
using VShop.DiscountApi.DTOs;

namespace VShop.DiscountApi.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CouponDTO, Coupon>().ReverseMap();
    }
}
