using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

/// <summary>
/// Configures the entity type <see cref="Video"/> for Entity Framework.
/// </summary>
public class VideoConfig : IEntityTypeConfiguration<Video>
{
    /// <summary>
    /// Configures the <see cref="Video"/> entity with the specified <see cref="EntityTypeBuilder{Video}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        // Define the primary key for the Video entity
        builder.HasKey(video => video.Id);

        // Define properties with required constraints
        builder.Property(video => video.SizeBytes)
            .IsRequired();

        builder.Property(video => video.Link)
            .IsRequired();

        builder.Property(video => video.VideoExtension)
            .IsRequired();

        builder.Property(video => video.AudioId)
            .IsRequired();

        // Specify the table name for the Video entity
        builder.ToTable("Videos");
    }
}
