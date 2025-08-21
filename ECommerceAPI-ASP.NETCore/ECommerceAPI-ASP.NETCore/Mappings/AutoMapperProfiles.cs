using AutoMapper;
using ECommerceAPI_ASP.NETCore.Models.Domain;
using ECommerceAPI_ASP.NETCore.Models.DTO.Category;

namespace ECommerceAPI_ASP.NETCore.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryRequestDto, Category>();
        }
    }
}
