using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(post => post.Id);

        builder.Property(post => post.Description)
            .IsRequired();

        builder.Property(post => post.Platform)
            .IsRequired();

        builder.HasOne(post => post.Header)
            .WithOne(header => header.Post)
            .HasForeignKey<Header>(header => header.PostId)
            .IsRequired(false);

        builder.HasMany(post => post.Images)
            .WithOne(image => image.Post)
            .HasForeignKey(image => image.PostId)
            .IsRequired();

        builder.ToTable("Posts");
    }
}
