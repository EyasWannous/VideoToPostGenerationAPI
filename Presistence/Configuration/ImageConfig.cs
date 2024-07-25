using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class ImageConfig : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(image => image.Id);

        builder.Property(image => image.SizeBytes)
            .IsRequired();

        builder.Property(image => image.Link)
            .IsRequired();

        builder.Property(image => image.ImageExtension)
            .IsRequired();

        builder.Property(image => image.PostId)
            .IsRequired();

        builder.ToTable("Images");
    }
}
