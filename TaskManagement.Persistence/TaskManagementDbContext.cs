using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain;
using TaskManagement.Domain.Common;

namespace TaskManagement.Persistence
{
    public class TaskManagementDbContext : IdentityDbContext<AppUser>
    {
          public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
           : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

              base.OnModelCreating(modelBuilder);


            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagementDbContext).Assembly);

             // Configure one-to-many relationship between Task and CheckList
            modelBuilder.Entity<Domain.Task>()
                .HasMany(t => t.CheckLists)
                .WithOne(cl => cl.Task)
                .HasForeignKey(cl => cl.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            
             // Configure the relationships
            modelBuilder.Entity<Domain.Task>()
                .HasOne(t => t.Creator)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.CreatorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CheckList>()
                .HasOne(cl => cl.Creator)
                .WithMany(u => u.CheckLists)
                .HasForeignKey(cl => cl.CreatorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        }

         public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var entry in ChangeTracker.Entries<BaseDomainEntity>())
            {
                entry.Entity.LastModifiedDate = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.DateCreated = DateTime.Now;
                }
            }  


            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<Domain.Task> Tasks { get; set; }

        public DbSet<CheckList> CheckLists { get; set; }



    }
}