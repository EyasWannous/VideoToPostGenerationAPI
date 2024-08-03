using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

/// <summary>
/// Configures the entity type <see cref="Post"/> for Entity Framework.
/// </summary>
public class PostConfig : IEntityTypeConfiguration<Post>
{
    /// <summary>
    /// Configures the <see cref="Post"/> entity with the specified <see cref="EntityTypeBuilder{Post}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        // Define the primary key for the Post entity
        builder.HasKey(post => post.Id);

        // Define properties with required constraints
        builder.Property(post => post.Description)
            .IsRequired();

        builder.Property(post => post.Platform)
            .IsRequired();

        // Configure the relationship between Post and Header
        builder.HasOne(post => post.Header)
            .WithOne(header => header.Post)
            .HasForeignKey<Header>(header => header.PostId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the relationship between Post and Images
        builder.HasMany(post => post.Images)
            .WithOne(image => image.Post)
            .HasForeignKey(image => image.PostId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Specify the table name for the Post entity
        builder.ToTable("Posts");
    }
}
