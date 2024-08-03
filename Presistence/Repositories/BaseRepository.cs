using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

/// <summary>
/// Base repository implementation for handling CRUD operations.
/// </summary>
public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    /// <summary>
    /// The database context for accessing the database.
    /// </summary>
    protected readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all entities of type T.
    /// </summary>
    /// <returns>A collection of entities.</returns>
    public async Task<IEnumerable<T>> GetAllAsync()
        => await _context.Set<T>()
            .AsNoTracking()
            .ToListAsync();

    /// <summary>
    /// Gets an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The entity, or null if not found.</returns>
    public async Task<T?> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);

    /// <summary>
    /// Finds a single entity based on the specified criteria.
    /// </summary>
    /// <param name="criteria">The criteria for finding the entity.</param>
    /// <param name="includes">Optional navigation properties to include in the query.</param>
    /// <returns>The entity that matches the criteria, or null if not found.</returns>
    public async Task<T?> FindAsync(Expression<Func<T, bool>> criteria, params string[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.SingleOrDefaultAsync(criteria);
    }

    /// <summary>
    /// Adds a new entity to the context.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The added entity.</returns>
    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// Deletes an entity from the context.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Counts all entities of type T.
    /// </summary>
    /// <returns>The number of entities.</returns>
    public async Task<int> CountAsync()
        => await _context.Set<T>().CountAsync();

    /// <summary>
    /// Counts entities based on the specified criteria.
    /// </summary>
    /// <param name="criteria">The criteria for counting the entities.</param>
    /// <returns>The number of entities that match the criteria.</returns>
    public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        => await _context.Set<T>().CountAsync(criteria);

    /// <summary>
    /// Paginates entities of type T.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of entities per page.</param>
    /// <param name="includes">Optional navigation properties to include in the query.</param>
    /// <returns>A collection of entities for the specified page.</returns>
    public async Task<IEnumerable<T>> PaginateAsync(int pageNumber = 1, int pageSize = 10, params string[]? includes)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Updates an existing entity in the context.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>The updated entity.</returns>
    public async Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return await Task.FromResult(entity);
    }
}
