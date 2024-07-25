using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class AudioConfig : IEntityTypeConfiguration<Audio>
{
    public void Configure(EntityTypeBuilder<Audio> builder)
    {
        builder.HasKey(audio => audio.Id);

        builder.Property(audio => audio.SizeBytes)
            .IsRequired();

        builder.Property(audio => audio.Link)
            .IsRequired();

        builder.Property(audio => audio.AudioExtension)
            .IsRequired();

        builder.Property(audio => audio.Duration)
            .IsRequired();

        builder.Property(audio => audio.UserId)
            .IsRequired();

        builder.HasOne(audio => audio.Video)
            .WithOne(video => video.Audio)
            .HasForeignKey<Video>(video => video.AudioId)
            .IsRequired(false);

        builder.HasMany(audio => audio.GeneralPosts)
            .WithOne(generalPost => generalPost.Audio)
            .HasForeignKey(generalPost => generalPost.AudioId)
            .IsRequired();

        builder.ToTable("Audios");
    }
}
