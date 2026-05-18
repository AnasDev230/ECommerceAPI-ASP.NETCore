using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO;
using ECommerceAPI_ASP.NETCore.Models.DTO.Category;
using ECommerceAPI_ASP.NETCore.Models.DTO.Address;
using ECommerceAPI_ASP.NETCore.Models.DTO.AuditLog;
using ECommerceAPI_ASP.NETCore.Models.DTO.Image;
using ECommerceAPI_ASP.NETCore.Models.DTO.Order;
using ECommerceAPI_ASP.NETCore.Models.DTO.Order.OrderItem;
using ECommerceAPI_ASP.NETCore.Models.DTO.Payment;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Rating;
using ECommerceAPI_ASP.NETCore.Models.DTO.Product.Stock;
using ECommerceAPI_ASP.NETCore.Models.DTO.Shipping;
using ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart;
using ECommerceAPI_ASP.NETCore.Models.DTO.ShoppingCart.ShoppingCartItem;
using ECommerceAPI_ASP.NETCore.Models.DTO.Transaction;

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

            CreateMap<Stock, StockDto>().ReverseMap();
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

            CreateMap<Image, ImageDto>().ReverseMap();

            CreateMap<Payment, PaymentDto>()
                .ForMember(d => d.Method, opt => opt.MapFrom(s => s.Method.ToString()))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

            CreateMap<Transaction, TransactionDto>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type.ToString()))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

            CreateMap<Shipping, ShippingDto>()
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

            CreateMap<Address, AddressDto>()
                .ForMember(d => d.AddressType, opt => opt.MapFrom(s => s.AddressType.ToString()));

            CreateMap<AuditLog, AuditLogDto>();
        }
    }
}
