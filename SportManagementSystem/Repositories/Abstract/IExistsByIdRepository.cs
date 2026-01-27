namespace SportManagementSystem.Repositories.Abstract;

public interface IExistsByIdRepository<T>
{
    Task<bool> ExistsAsync(long id, CancellationToken ct);
}