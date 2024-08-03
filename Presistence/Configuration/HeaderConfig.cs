using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

/// <summary>
/// Configures the entity type <see cref="Header"/> for Entity Framework.
/// </summary>
public class HeaderConfig : IEntityTypeConfiguration<Header>
{
    /// <summary>
    /// Configures the <see cref="Header"/> entity with the specified <see cref="EntityTypeBuilder{Header}"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Header> builder)
    {
        // Define the primary key for the Header entity
        builder.HasKey(header => header.Id);

        // Define properties with required constraints
        builder.Property(header => header.Title)
            .IsRequired();

        builder.Property(header => header.PostId)
            .IsRequired();

        // Specify the table name for the Header entity
        builder.ToTable("Headers");
    }
}
