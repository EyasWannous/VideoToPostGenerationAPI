using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class VideoConfig : IEntityTypeConfiguration<Video>
{
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
