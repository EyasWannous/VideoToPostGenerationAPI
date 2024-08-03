using AutoMapper;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.DTOs.Outgoing;

namespace VideoToPostGenerationAPI.Services.Helpers;

/// <summary>
/// AutoMapper profile to configure mappings between domain entities and DTOs.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// Configures the mappings between domain entities and DTOs.
    /// </summary>
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
    }
}
