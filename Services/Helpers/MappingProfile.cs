using AutoMapper;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Services.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Video, ResponseVideoDTO>();
        
        CreateMap<Post, ResponsePostDTO>();
    }
}
