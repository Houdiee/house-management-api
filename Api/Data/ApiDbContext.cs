using HouseManagementApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseManagementApi.Data;

public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<House> Houses { get; set; }
    public DbSet<HouseUser> HouseUsers { get; set; }
    public DbSet<DutyTemplate> DutyTemplates { get; set; }
    public DbSet<DutyInstance> DutyInstances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // A House has many HouseUsers
        modelBuilder.Entity<House>()
            .HasMany(h => h.HouseUsers)
            .WithOne(hu => hu.House)
            .HasForeignKey(hu => hu.HouseId)
            .OnDelete(DeleteBehavior.Cascade);

        // A User has many HouseUsers
        modelBuilder.Entity<User>()
            .HasMany(u => u.HouseUsers)
            .WithOne(hu => hu.User)
            .HasForeignKey(hu => hu.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Configure a composite primary key for the join table
        modelBuilder.Entity<HouseUser>()
            .HasKey(hu => new { hu.HouseId, hu.UserId });

        // A House has many DutyTemplates
        modelBuilder.Entity<DutyTemplate>()
            .HasOne(dt => dt.House)
            .WithMany(h => h.DutyTemplates)
            .HasForeignKey(dt => dt.HouseId)
            .OnDelete(DeleteBehavior.Cascade);

        // A DutyTemplate can have many DutyInstances
        modelBuilder.Entity<DutyInstance>()
            .HasOne(di => di.Template)
            .WithMany(t => t.DutyInstances)
            .HasForeignKey(di => di.TemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        // A DutyInstance is assigned to one User
        modelBuilder.Entity<DutyInstance>()
            .HasOne(di => di.AssignedUser)
            .WithMany(u => u.DutyInstances)
            .HasForeignKey(di => di.AssignedUserId);

        base.OnModelCreating(modelBuilder);
    }
}
