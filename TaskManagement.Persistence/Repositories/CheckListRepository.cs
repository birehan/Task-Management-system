using TaskManagement.Application.Contracts.Persistence;

namespace TaskManagement.Persistence.Repositories
{
    public class CheckListRepository : GenericRepository<Domain.CheckList>, ICheckListRepository
    {
          private readonly TaskManagementDbContext _dbContext;

          public CheckListRepository(TaskManagementDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}