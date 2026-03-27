using Microsoft.EntityFrameworkCore;
using SportManagementSystem.Entities.Enums;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Enums;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Enums;
using SportManagementSystem.Modules.Images.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Enums;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Enums;

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
    public DbSet<DbMembershipTemplate> MembershipTemplates => Set<DbMembershipTemplate>();
    public DbSet<DbBranch> Branches => Set<DbBranch>();
    public DbSet<DbRoom> Rooms => Set<DbRoom>();
    public DbSet<DbEquipment> Equipments => Set<DbEquipment>();
    public DbSet<DbImage> Images => Set<DbImage>();

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

        modelBuilder.Entity<DbBranch>()
            .HasOne(x => x.Admin)
            .WithMany(x => x.CreatedBranches)
            .HasForeignKey(x => x.AdminId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DbBranch>()
            .HasMany(x => x.BranchAdmins)
            .WithMany(x => x.AdminBranches)
            .UsingEntity<Dictionary<string, object>>(
                "BranchAdmins",
                right => right
                    .HasOne<DbStaff>()
                    .WithMany()
                    .HasForeignKey("StaffId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<DbBranch>()
                    .WithMany()
                    .HasForeignKey("BranchId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.HasKey("BranchId", "StaffId");
                    join.ToTable("BranchAdmins");
                });

        modelBuilder.Entity<DbBranch>()
            .HasMany(x => x.BranchStaffs)
            .WithMany(x => x.StaffBranches)
            .UsingEntity<Dictionary<string, object>>(
                "BranchStaffs",
                right => right
                    .HasOne<DbStaff>()
                    .WithMany()
                    .HasForeignKey("StaffId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left
                    .HasOne<DbBranch>()
                    .WithMany()
                    .HasForeignKey("BranchId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.HasKey("BranchId", "StaffId");
                    join.ToTable("BranchStaffs");
                });

        modelBuilder.Entity<DbSportService>()
            .HasOne(x => x.Branch)
            .WithMany(x => x.SportServices)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DbMembershipTemplate>()
            .HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DbMembershipTemplate>()
            .HasOne(x => x.SportService)
            .WithMany()
            .HasForeignKey(x => x.SportServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DbMembershipTemplate>()
            .HasOne(x => x.ServicePrice)
            .WithMany()
            .HasForeignKey(x => x.ServicePriceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DbMembership>()
            .HasOne(x => x.MembershipTemplate)
            .WithMany(x => x.Memberships)
            .HasForeignKey(x => x.MembershipTemplateId)
            .OnDelete(DeleteBehavior.SetNull);
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
