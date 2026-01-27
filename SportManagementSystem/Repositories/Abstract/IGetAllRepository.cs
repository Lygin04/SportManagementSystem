namespace SportManagementSystem.Repositories.Abstract;

public interface IGetAllRepository<T>
{
    Task<List<T>> GetAllAsync(CancellationToken ct);
}