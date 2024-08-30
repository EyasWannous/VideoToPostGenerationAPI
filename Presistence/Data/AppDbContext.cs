using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VideoToPostGenerationAPI.Domain.Entities;

namespace VideoToPostGenerationAPI.Presistence.Data;

/// <summary>
/// Represents the application database context for Entity Framework.
/// Inherits from <see cref="IdentityDbContext"/> to provide support for Identity.
/// </summary>
public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for configuring the database context.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{Video}"/> representing the Videos in the database.
    /// </summary>
    public DbSet<Video> Videos { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{Audio}"/> representing the Audios in the database.
    /// </summary>
    public DbSet<Audio> Audios { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{Post}"/> representing the Posts in the database.
    /// </summary>
    public DbSet<Post> Posts { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{Image}"/> representing the Images in the database.
    /// </summary>
    public DbSet<PostImage> Images { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{Header}"/> representing the Headers in the database.
    /// </summary>
    public DbSet<Header> Headers { get; set; }

    public DbSet<PostOptions> PostOptions { get; set; }

    public DbSet<VideoThumbnail> Thumbnails { get; set; }

    /// <summary>
    /// Configures the model creating process for the context.
    /// Applies configurations from the assembly where the context is defined.
    /// </summary>
    /// <param name="modelBuilder">The builder used to configure the model.</param>
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
