using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class PostImageConfig : IEntityTypeConfiguration<PostImage>
{
    public void Configure(EntityTypeBuilder<PostImage> builder)
    {
        // Define the primary key for the Image entity
        builder.HasKey(postImage => postImage.Id);

        // Define properties with required constraints
        builder.Property(postImage => postImage.SizeBytes)
            .IsRequired();

        builder.Property(postImage => postImage.Link)
            .IsRequired();

        builder.Property(postImage => postImage.ImageExtension)
            .IsRequired();

        builder.Property(postImage => postImage.PostId)
            .IsRequired();

        // Specify the table name for the Image entity
        builder.ToTable("PostImages");
    }
}
