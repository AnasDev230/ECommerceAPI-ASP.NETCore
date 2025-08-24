using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Category;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Stock;

namespace ECommerceAPI_ASP.NETCore.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryRequestDto, Category>();

            CreateMap<Product,ProductDto>().ReverseMap();
            CreateMap<CreateProductRequestDto, Product>();

            CreateMap<Stock,StockDto>().ReverseMap();   
            CreateMap<CreateStockRequestDto, Stock>();
            CreateMap<UpdateStockRequestDto, Stock>();
        }
    }
}
