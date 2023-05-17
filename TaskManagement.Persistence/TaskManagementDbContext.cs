using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain;
using TaskManagement.Domain.Common;

namespace TaskManagement.Persistence
{
    public class TaskManagementDbContext : DbContext
    {
          public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
           : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagementDbContext).Assembly);

             // Configure one-to-many relationship between Task and CheckList
        modelBuilder.Entity<Domain.Task>()
            .HasMany(t => t.CheckLists)
            .WithOne(cl => cl.Task)
            .HasForeignKey(cl => cl.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

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