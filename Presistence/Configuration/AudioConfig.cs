using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class AudioConfig : IEntityTypeConfiguration<Audio>
{
    public void Configure(EntityTypeBuilder<Audio> builder)
    {
        // Define the primary key for the Audio entity
        builder.HasKey(audio => audio.Id);

        // Define properties with required constraints
        builder.Property(audio => audio.SizeBytes)
            .IsRequired();

        builder.Property(audio => audio.Link)
            .IsRequired();

        builder.Property(audio => audio.AudioExtension)
            .IsRequired();

        builder.Property(audio => audio.Duration)
            .IsRequired();

        builder.Property(audio => audio.HasVideo)
            .IsRequired();

        builder.Property(audio => audio.UserId)
            .IsRequired();

        // Configure the relationship between Audio and Video
        builder.HasOne(audio => audio.Video)
            .WithOne(video => video.Audio)
            .HasForeignKey<Video>(video => video.AudioId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the relationship between Audio and Posts
        builder.HasMany(audio => audio.Posts)
            .WithOne(posts => posts.Audio)
            .HasForeignKey(posts => posts.AudioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the relationship between Audio and VideoThumbnail
        builder.HasOne(audio => audio.VideoThumbnail)
            .WithOne(videoThumbnail => videoThumbnail.Audio)
            .HasForeignKey<VideoThumbnail>(videoThumbnail => videoThumbnail.AudioId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        // Specify the table name for the Audio entity
        builder.ToTable("Audios");
    }
}
