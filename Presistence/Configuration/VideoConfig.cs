using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class VideoConfig : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.HasKey(video => video.Id);

        builder.Property(video => video.SizeBytes)
            .IsRequired();

        builder.Property(video => video.Link)
            .IsRequired();

        builder.Property(video => video.VideoExtension)
            .IsRequired();

        builder.Property(video => video.AudioId)
            .IsRequired();

        builder.ToTable("Videos");
    }
}
