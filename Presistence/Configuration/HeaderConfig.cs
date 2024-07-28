using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class HeaderConfig : IEntityTypeConfiguration<Header>
{
    public void Configure(EntityTypeBuilder<Header> builder)
    {
        builder.HasKey(header => header.Id);

        builder.Property(header => header.Title)
            .IsRequired();

        builder.Property(header => header.PostId)
            .IsRequired();

        builder.ToTable("Headers");
    }
}
