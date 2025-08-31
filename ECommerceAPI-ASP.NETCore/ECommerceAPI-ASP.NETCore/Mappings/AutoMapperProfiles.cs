using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Category;
using ECommerceAPI_ASP.NETCore.Models.DTO.Order;
using ECommerceAPI_ASP.NETCore.Models.DTO.Order.OrderItem;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Rating;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Stock;
using ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart;
using ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart.ShoppingCartItem;

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

            CreateMap<ShoppingCart,ShoppingCartDto>().ReverseMap();
            CreateMap<CreateShoppingCartRequestDto, ShoppingCart>();

            CreateMap<ShoppingCartItem,ShoppingCartItemDto>().ReverseMap();
            CreateMap<CreateShoppingCartItemRequestDto, ShoppingCartItem>();
            CreateMap<UpdateShoppingCartItemRequestDto, ShoppingCartItem>();


            CreateMap<Rating,RatingDto>().ReverseMap();
            CreateMap<CreateRatingRequestDto, Rating>();
            CreateMap<UpdateRatingRequestDto, Rating>();

            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Order,OrderDto>().ReverseMap();
        }
    }
}
