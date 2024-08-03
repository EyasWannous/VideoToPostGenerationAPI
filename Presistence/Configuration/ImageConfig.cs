using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

/// <summary>
/// Configures the entity type <see cref="Image"/> for Entity Framework.
/// </summary>
public class ImageConfig : IEntityTypeConfiguration<Image>
{
    /// <summary>
    /// Configures the <see cref="Image"/> entity with the specified <see cref="EntityTypeBuilder{Image}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        // Define the primary key for the Image entity
        builder.HasKey(image => image.Id);

        // Define properties with required constraints
        builder.Property(image => image.SizeBytes)
            .IsRequired();

        builder.Property(image => image.Link)
            .IsRequired();

        builder.Property(image => image.ImageExtension)
            .IsRequired();

        builder.Property(image => image.PostId)
            .IsRequired();

        // Specify the table name for the Image entity
        builder.ToTable("Images");
    }
}
