using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Presistence.Repositories;

namespace VideoToPostGenerationAPI.Presistence.Data;

/// <summary>
/// Unit of Work class that encapsulates the repositories and manages the database context.
/// </summary>
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private bool _disposed = false;
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        Audios = new AudioRepository(_context);
        Videos = new VideoRepository(_context);
        Headers = new HeaderRepository(_context);
        Posts = new PostRepository(_context);
        PostsImages = new PostImageRepository(_context);
        PostsOptions = new PostOptionsRepository(_context);
        VideoThumbnails = new VideoThumbnailRepository(_context);
    }

    /// <summary>
    /// Gets the user repository.
    /// </summary>
    public IUserRepository Users { get; }

    /// <summary>
    /// Gets the audio repository.
    /// </summary>
    public IAudioRepository Audios { get; }

    /// <summary>
    /// Gets the video repository.
    /// </summary>
    public IVideoRepository Videos { get; }

    /// <summary>
    /// Gets the header repository.
    /// </summary>
    public IHeaderRepository Headers { get; }

    /// <summary>
    /// Gets the post repository.
    /// </summary>
    public IPostRepository Posts { get; }

    /// <summary>
    /// Gets the image repository.
    /// </summary>
    public IPostImageRepository PostsImages { get; }

    public IPostOptionsRepository PostsOptions { get; }

    public IVideoThumbnailRepository VideoThumbnails { get; }


    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="UnitOfWork"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">A boolean value indicating whether to release both managed and unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Destructor for the <see cref="UnitOfWork"/> class.
    /// </summary>
    ~UnitOfWork()
    {
        Dispose(false);
    }
}
