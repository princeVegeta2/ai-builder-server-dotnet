using AutoMapper;
using AIBuilderServerDotnet.DTOs;
using AIBuilderServerDotnet.Models;

namespace AIBuilderServerDotnet.Mappings
{
    public class ProjectMappingsProfile : Profile
    {
        public ProjectMappingsProfile()
        {
            CreateMap<AddProjectDto, Project>();
        }
    }
}
