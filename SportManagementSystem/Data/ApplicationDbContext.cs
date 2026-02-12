using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Entities.Enums;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Entities;

namespace SportManagementSystem.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<DbClient> Clients => Set<DbClient>();
    public DbSet<DbStaff> Staffs => Set<DbStaff>();
    public DbSet<DbUserAccount> UserAccounts => Set<DbUserAccount>();
    public DbSet<DbSportService> SportServices => Set<DbSportService>();
    public DbSet<DbServicePrice> ServicePrices => Set<DbServicePrice>();
    public DbSet<DbServiceSchedule> ServiceSchedules => Set<DbServiceSchedule>();
    public DbSet<DbTrainingSession> TrainingSessions => Set<DbTrainingSession>();
    public DbSet<DbBooking> Bookings => Set<DbBooking>();
    public DbSet<DbMembership> Memberships => Set<DbMembership>();
    public DbSet<DbBranch> Branches => Set<DbBranch>();
    public DbSet<DbRoom> Rooms => Set<DbRoom>();
    public DbSet<DbEquipment> Equipments => Set<DbEquipment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<EAccountStatus>();
        modelBuilder.HasPostgresEnum<EBookingStatus>();
        modelBuilder.HasPostgresEnum<EEquipmentCondition>();
        modelBuilder.HasPostgresEnum<EIsoCurrency>();
        modelBuilder.HasPostgresEnum<EMembershipStatus>();
        modelBuilder.HasPostgresEnum<ENotificationStatus>();
        modelBuilder.HasPostgresEnum<ERoomStatus>();
        modelBuilder.HasPostgresEnum<EServiceCategory>();
        modelBuilder.HasPostgresEnum<ESessionStatus>();
        modelBuilder.HasPostgresEnum<EUserRole>();
        
        modelBuilder.Entity<DbSportService>()
            .HasIndex(x => x.Code)
            .IsUnique();
        
        modelBuilder.Entity<DbUserAccount>()
            .HasIndex(x => x.Email)
            .IsUnique();
    }
    
    public void BeginTransaction()
    {
        Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        Database.CommitTransaction();
    }
    
    public void RollbackTransaction()
    {
        Database.RollbackTransaction();
    }
}