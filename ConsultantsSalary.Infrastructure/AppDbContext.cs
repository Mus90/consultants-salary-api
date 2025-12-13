using ConsultantsSalary.Domain.Entities;
using ConsultantsSalary.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConsultantsSalary.Infrastructure;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Consultant> Consultants => Set<Consultant>();
    public DbSet<Role> ConsultantRoles => Set<Role>();
    public DbSet<RoleRateHistory> RoleRateHistories => Set<RoleRateHistory>();
    public DbSet<Domain.Entities.Task> Tasks => Set<Domain.Entities.Task>();
    public DbSet<ConsultantTaskAssignment> ConsultantTaskAssignments => Set<ConsultantTaskAssignment>();
    public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>(b =>
        {
            b.Property(x => x.Name).IsRequired().HasMaxLength(100);
            b.HasMany(x => x.RateHistory)
             .WithOne(x => x.Role)
             .HasForeignKey(x => x.RoleId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RoleRateHistory>(b =>
        {
            b.Property(x => x.RatePerHour).HasColumnType("decimal(18,2)");
            b.Property(x => x.EffectiveDate).IsRequired();
            b.HasIndex(x => new { x.RoleId, x.EffectiveDate });
        });

        modelBuilder.Entity<Consultant>(b =>
        {
            b.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            b.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            b.Property(x => x.Email).IsRequired().HasMaxLength(200);
            b.HasIndex(x => x.Email).IsUnique();
            b.HasOne(x => x.Role)
             .WithMany(r => r.Consultants)
             .HasForeignKey(x => x.RoleId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Domain.Entities.Task>(b =>
        {
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<ConsultantTaskAssignment>(b =>
        {
            b.HasKey(x => new { x.ConsultantId, x.TaskId });
            b.HasOne(x => x.Consultant)
             .WithMany(c => c.TaskAssignments)
             .HasForeignKey(x => x.ConsultantId)
             .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(x => x.Task)
             .WithMany(t => t.Assignments)
             .HasForeignKey(x => x.TaskId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TimeEntry>(b =>
        {
            b.Property(x => x.HoursWorked).HasColumnType("decimal(5,2)");
            b.Property(x => x.DateWorked).HasColumnType("date");
            b.HasOne(x => x.Consultant)
             .WithMany(c => c.TimeEntries)
             .HasForeignKey(x => x.ConsultantId)
             .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(x => x.Task)
             .WithMany(t => t.TimeEntries)
             .HasForeignKey(x => x.TaskId)
             .OnDelete(DeleteBehavior.Cascade);
            b.HasOne(x => x.RateSnapshot)
             .WithMany()
             .HasForeignKey(x => x.RateSnapshotId)
             .OnDelete(DeleteBehavior.Restrict);
            b.HasIndex(x => new { x.ConsultantId, x.DateWorked });
        });
    }
}
