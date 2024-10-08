﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Configure the one-to-many relationship between User and Audio
        builder.HasMany(user => user.Audios)
            .WithOne(audio => audio.User)
            .HasForeignKey(audio => audio.UserId)
            .IsRequired();

        // Specify the table name for the User entity
        builder.ToTable("Users");

        // Optionally, if needed, configure ignored properties as shown below
        // builder.Ignore(user => user.EmailConfirmed);
        // builder.Ignore(user => user.NormalizedEmail);
        // builder.Ignore(user => user.NormalizedUserName);
        // builder.Ignore(user => user.UserName);
        // builder.Ignore(user => user.PhoneNumber);
        // builder.Ignore(user => user.PhoneNumberConfirmed);
        // builder.Ignore(user => user.LockoutEnd);
        // builder.Ignore(user => user.LockoutEnabled);
        // builder.Ignore(user => user.TwoFactorEnabled);
        // builder.Ignore(user => user.AccessFailedCount);
        // builder.Ignore(user => user.SecurityStamp);
    }
}
