using SportManagementSystem.Data;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Infrastructure.Repositories;

public class BookingRepository(ApplicationDbContext db) : IBookingRepository
{
    public async Task<DbBooking> CreateAsync(DbBooking entity, CancellationToken ct)
    {
        var booking = await db.Bookings.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return booking.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var booking = new  DbBooking { Id = id };
        db.Bookings.Attach(booking);
        db.Bookings.Remove(booking);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbBooking?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.Bookings.FindAsync(id, ct);
    }
}