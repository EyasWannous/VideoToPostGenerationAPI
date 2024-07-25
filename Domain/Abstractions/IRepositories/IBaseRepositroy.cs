using System.Linq.Expressions;

namespace VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> Paginate(int pageNumber = 1, int pageSize = 10, string[]? includes = null);
    Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[]? includes = null);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> criteria);
}
