using VideoToPostGenerationAPI.Domain.Abstractions;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Presistence.Repositories;

namespace VideoToPostGenerationAPI.Presistence.Data;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private bool _disposed = false;
    private readonly AppDbContext _context = context;
    public IUserRepository Users { get; } = new UserRepository(context);

    public IVideoRepository Videos { get; }  = new VideoRepository(context);

    public IAudioRepository Audios { get; } = new AudioRepository(context);

    public IHeaderRepository Headers { get; } = new HeaderRepository(context);

    public IPostRepository Posts { get; } = new PostRepository(context);

    public IImageRepository Images { get; } = new ImageRepository(context);

    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    // disposing : true (dispose managed + unmanaged)
    // disposing : false (dispose unmanaged + large fields)
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        // Dispose Logic
        if (disposing)
        {
            //Dispose Managed Resources
            _context.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()   
    {
        // Disopose is 100% calling
        Dispose(true);

        // notify garbage collector to not call destructor
        GC.SuppressFinalize(this);
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}
