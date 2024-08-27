using AutoMapper;
using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Mappings
{
    public class PageMappingProfile : Profile
    {
        public PageMappingProfile()
        {
            // Map from AddPageDto to Page, ignoring the Id property
            CreateMap<AddPageDto, Page>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // Ignore CreatedAt if you want it to be set automatically

            CreateMap<UpdatePageDto, Page>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NewName))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
