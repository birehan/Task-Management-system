using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagement.Persistence.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Domain.Task>
    {
        public void Configure(EntityTypeBuilder<Domain.Task> builder)
        {
             builder.HasData(
                  new Domain.Task
                       {
                           Title = "title",
                           Description = "Sample Content",
                           Status = false,
                           StartDate = new DateTime(),
                           EndDate = new DateTime(),
                            Id = 1
                       },

                  new Domain.Task
                       {
                           Title = "title 2",
                           Description = "Sample Content 2",
                           Status = false,
                           StartDate = new DateTime(),
                           EndDate = new DateTime(),
                            Id = 2
                       }
                );

        }
    }
}