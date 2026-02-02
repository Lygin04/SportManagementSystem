namespace SportManagementSystem.Repositories.Abstract;

public interface IBaseRepository<T>
{
    Task<T> CreateAsync(T entity, CancellationToken ct);
    Task DeleteAsync(long id, CancellationToken ct);
    Task<T?> GetByIdAsync(long id, CancellationToken ct);
}