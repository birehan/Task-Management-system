using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Persistence.Repositories;
using TaskManagement.Application.Contracts.Persistence;

namespace TaskManagement.Persistence
{
    public static class PersistenceServicesRegistration
    {
         public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TaskManagementDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("TaskManagementConnectionString")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ICheckListRepository ,CheckListRepository >();
            return services;
        }

    }
}