using AutoMapper;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Services.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map Audio entity to ResponseAudioDTO
        CreateMap<Audio, ResponseAudioDTO>()
            .ReverseMap(); // Optional: If you want to map from DTO back to entity

        // Map Video entity to ResponseVideoDTO
        CreateMap<Video, ResponseVideoDTO>()
            .ReverseMap(); // Optional: If you want to map from DTO back to entity

        // Map Post entity to ResponsePostDTO
        CreateMap<Post, ResponsePostDTO>()
            .ReverseMap(); // Optional: If you want to map from DTO back to entity

        // Map Header entity to ResponseHeaderDTO
        CreateMap<Header, ResponseHeaderDTO>()
            .ReverseMap(); // Optional: If you want to map from DTO back to entity

        CreateMap<VideoThumbnail, ResponseVideoThumbnailDTO>()
            .ReverseMap();

        CreateMap<PostOptions, ResponsePostOptionsDTO>()
            .ReverseMap();

        CreateMap<PostImage, ResponsePostImageDTO>()
            .ReverseMap();
    }

}
