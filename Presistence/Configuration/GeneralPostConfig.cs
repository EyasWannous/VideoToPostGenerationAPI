using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class GeneralPostConfig : IEntityTypeConfiguration<GeneralPost>
{
    public void Configure(EntityTypeBuilder<GeneralPost> builder)
    {
        builder.HasKey(generalPost => generalPost.Id);

        builder.Property(generalPost => generalPost.Title)
            .IsRequired();

        builder.Property(generalPost => generalPost.Description)
            .IsRequired();

        builder.Property(generalPost => generalPost.AudioId)
            .IsRequired();

        builder.HasMany(generalPost => generalPost.Posts)
            .WithOne(post => post.GeneralPost)
            .HasForeignKey(post => post.GeneralPostId)
            .IsRequired();

        builder.ToTable("GeneralPosts");
    }
}
