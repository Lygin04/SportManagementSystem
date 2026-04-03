using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Repositories.Abstract;

namespace SportManagementSystem.Modules.Clients.Domain.Repositories;

public interface IBookingRepository : IBaseRepository<DbBooking>
{
    Task<List<DbBooking>> GetByClientIdAsync(long clientId, CancellationToken ct);
}
