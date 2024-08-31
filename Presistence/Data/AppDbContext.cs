using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Video> Videos { get; set; }

    public DbSet<Audio> Audios { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<PostImage> Images { get; set; }

    public DbSet<Header> Headers { get; set; }

    public DbSet<PostOptions> PostOptions { get; set; }

    public DbSet<VideoThumbnail> Thumbnails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    // The OnConfiguring method is commented out as it's often used for configuration in development or testing
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     if (!optionsBuilder.IsConfigured)
    //     {
    //         optionsBuilder.UseSqlServer(""); // Connection string for the database
    //     }
    // }
}
