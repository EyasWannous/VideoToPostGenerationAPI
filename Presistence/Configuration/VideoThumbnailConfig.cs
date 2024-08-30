using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class VideoThumbnailConfig : IEntityTypeConfiguration<VideoThumbnail>
{
    public void Configure(EntityTypeBuilder<VideoThumbnail> builder)
    {
        builder.HasKey(videoThumbnail => videoThumbnail.Id);

        //builder.Property(videoThumbnail => videoThumbnail.SizeBytes)
        //    .IsRequired();

        builder.Property(videoThumbnail => videoThumbnail.Link)
            .IsRequired();

        //builder.Property(videoThumbnail => videoThumbnail.ImageExtension)
        //    .IsRequired();

        builder.ToTable("VideoThumbnails");
    }
}
