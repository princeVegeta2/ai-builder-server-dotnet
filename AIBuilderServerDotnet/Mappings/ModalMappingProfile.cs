using AutoMapper;
using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Mappings
{
    public class ModalMappingProfile : Profile
    {
        public ModalMappingProfile()
        {
            CreateMap<AddModalDto, ColorModal>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<AddModalDto, LinkModal>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<AddModalDto, ImageLinkModal>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<AddModalDto, PromptModal>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        }
    }
}
