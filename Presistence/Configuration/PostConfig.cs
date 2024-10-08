﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class PostConfig : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        // Define the primary key for the Post entity
        builder.HasKey(post => post.Id);

        // Define properties with required constraints
        builder.Property(post => post.Description)
            .IsRequired();

        // Configure the relationship between Post and Header
        builder.HasOne(post => post.Header)
            .WithOne(header => header.Post)
            .HasForeignKey<Header>(header => header.PostId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the relationship between Post and Images
        builder.HasMany(post => post.PostImages)
            .WithOne(image => image.Post)
            .HasForeignKey(image => image.PostId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(post => post.PostOptions)
            .WithOne(postOptions => postOptions.Post)
            .HasForeignKey<PostOptions>(postOptions => postOptions.PostId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Specify the table name for the Post entity
        builder.ToTable("Posts");
    }
}
