using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Presistence.Repositories;

namespace VideoToPostGenerationAPI.Presistence.Data;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private bool _disposed = false;
    private readonly AppDbContext _context;

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

    public IUserRepository Users { get; }

    public IAudioRepository Audios { get; }

    public IVideoRepository Videos { get; }

    public IHeaderRepository Headers { get; }

    public IPostRepository Posts { get; }

    public IPostImageRepository PostsImages { get; }

    public IPostOptionsRepository PostsOptions { get; }

    public IVideoThumbnailRepository VideoThumbnails { get; }


    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}
