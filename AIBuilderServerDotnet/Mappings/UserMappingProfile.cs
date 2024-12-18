﻿using AutoMapper;
using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // Define mappings for user-related DTOs and models
            CreateMap<SignUpDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());// PasswordHash will be set separately

            CreateMap<SignInDto, User>();

            // Mapping user back to dtos
            CreateMap<User, SignUpDto>();
            CreateMap<User, SignInDto>();
        }
    }
}
