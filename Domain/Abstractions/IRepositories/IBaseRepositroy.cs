using System.Linq.Expressions;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

/// <summary>
/// Interface for the base repository providing common data access methods for entities.
/// </summary>
/// <typeparam name="T">The type of the entity that the repository manages. Must be a class.</typeparam>
public interface IBaseRepository<T> where T : class
{
    /// <summary>
    /// Retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the entity if found, or <c>null</c> if no entity with the specified ID exists.</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all entities from the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of all entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Retrieves a paginated list of entities from the repository.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve. Defaults to 1.</param>
    /// <param name="pageSize">The number of entities per page. Defaults to 10.</param>
    /// <param name="includes">Optional related entities to include in the query.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a collection of entities for the specified page.</returns>
    Task<IEnumerable<T>> PaginateAsync(int pageNumber = 1, int pageSize = 10, string[]? includes = null);

    /// <summary>
    /// Finds an entity based on the specified criteria.
    /// </summary>
    /// <param name="criteria">The criteria used to find the entity.</param>
    /// <param name="includes">Optional related entities to include in the query.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the entity if found, or <c>null</c> if no entity matches the criteria.</returns>
    Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[]? includes = null);

    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the added entity.</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated entity.</returns>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Counts the total number of entities in the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the total number of entities.</returns>
    Task<int> CountAsync();

    /// <summary>
    /// Counts the number of entities that match the specified criteria.
    /// </summary>
    /// <param name="criteria">The criteria used to count the entities.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the number of entities matching the criteria.</returns>
    Task<int> CountAsync(Expression<Func<T, bool>> criteria);
}
