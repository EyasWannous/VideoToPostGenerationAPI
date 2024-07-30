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

        builder.Property(video => video.Duration)
            .IsRequired();

        builder.Property(video => video.UserId)
            .IsRequired();

        builder.HasOne(video => video.Audio)
            .WithOne(audio => audio.Video)
            .HasForeignKey<Audio>(audio => audio.VideoId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(video => video.Posts)
            .WithOne(posts => posts.Audio)
            .HasForeignKey(posts => posts.AudioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("Videos");
    }
}
