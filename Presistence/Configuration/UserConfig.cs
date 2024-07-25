using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Configuration;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //builder.Ignore(user => user.EmailConfirmed);

        //builder.Ignore(user => user.NormalizedEmail);

        //builder.Ignore(user => user.NormalizedUserName);

        //builder.Ignore(user => user.UserName);

        //builder.Ignore(user => user.PhoneNumber);

        //builder.Ignore(user => user.PhoneNumberConfirmed);

        //builder.Ignore(user => user.LockoutEnd);

        //builder.Ignore(user => user.LockoutEnabled);

        //builder.Ignore(user => user.TwoFactorEnabled);

        //builder.Ignore(user => user.AccessFailedCount);

        //builder.Ignore(user => user.SecurityStamp);

        builder.HasMany(user => user.Audios)
            .WithOne(audio => audio.User)
            .HasForeignKey(audio => audio.UserId)
            .IsRequired();

        builder.ToTable("Users");
    }
}
