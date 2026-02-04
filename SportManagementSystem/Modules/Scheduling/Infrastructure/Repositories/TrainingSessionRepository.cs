using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;

namespace SportManagementSystem.Modules.Scheduling.Infrastructure.Repositories;

public class TrainingSessionRepository(ApplicationDbContext db) : ITrainingSessionRepository
{
    public async Task<DbTrainingSession> CreateAsync(DbTrainingSession entity, CancellationToken ct)
    {
        var trainingSession = await db.TrainingSessions.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
        return trainingSession.Entity;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var trainingSession = new DbTrainingSession { Id = id };
        db.TrainingSessions.Attach(trainingSession);
        db.TrainingSessions.Remove(trainingSession);
        await db.SaveChangesAsync(ct);
    }

    public async Task<DbTrainingSession?> GetByIdAsync(long id, CancellationToken ct)
    {
        return await db.TrainingSessions.FindAsync(id, ct);
    }

    public async Task<bool> ExistsAsync(long id, CancellationToken ct)
    {
        return await db.TrainingSessions.AnyAsync(x => x.Id == id, ct);
    }
}