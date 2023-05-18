using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Contracts.Persistence;

namespace TaskManagement.Persistence.Repositories
{
    public class TaskRepository : GenericRepository<Domain.Task>, ITaskRepository
    {
        
        private readonly TaskManagementDbContext _dbContext;

          public TaskRepository(TaskManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

         public async Task<IReadOnlyList<Domain.Task>> GetAll()
        {
            return await _dbContext.Set<Domain.Task>().Include(x => x.Creator).Include(x => x.CheckLists).AsNoTracking().ToListAsync();
        }


        public async Task<Domain.Task> Get(int id)
        {
            return await _dbContext.Set<Domain.Task>().Include(x => x.Creator).Include(x => x.CheckLists).FirstOrDefaultAsync(b => b.Id == id);
        }

    }
}