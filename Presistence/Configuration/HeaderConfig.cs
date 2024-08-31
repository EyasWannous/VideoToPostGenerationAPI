using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class HeaderConfig : IEntityTypeConfiguration<Header>
{
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
