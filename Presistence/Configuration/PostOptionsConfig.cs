using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class PostOptionsConfig : IEntityTypeConfiguration<PostOptions>
{
    public void Configure(EntityTypeBuilder<PostOptions> builder)
    {
        // Define the primary key for the Post entity
        builder.HasKey(postOptions => postOptions.Id);

        builder.Property(postOptions => postOptions.Platform)
            .IsRequired();

        builder.Property(postOptions => postOptions.PostId)
            .IsRequired();

        //builder.Property(postOptions => postOptions.PrimaryKeyPhrase)
        //    .IsRequired();

        builder.Property(postOptions => postOptions.PointOfView)
            .IsRequired();
        
        builder.Property(postOptions => postOptions.PostFormat)
            .IsRequired();

        builder.Property(postOptions => postOptions.UseEmojis)
            .IsRequired();

        builder.Property(postOptions => postOptions.AdditionalPrompt)
            .IsRequired();

        builder.Property(postOptions => postOptions.WordCount)
            .IsRequired();

        // Specify the table name for the Post entity
        builder.ToTable("PostsOptions");
    }
}
