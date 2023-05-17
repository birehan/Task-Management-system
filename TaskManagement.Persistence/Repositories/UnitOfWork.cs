
using TaskManagement.Application.Contracts.Persistence;

namespace TaskManagement.Persistence.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly TaskManagementDbContext _context;

        private ITaskRepository _taskRepository;
        private ICheckListRepository _checkListRepository;

          public UnitOfWork(TaskManagementDbContext context)
        {
            _context = context;
        }


        public ITaskRepository TaskRepository
        {
            get
            {
                return _taskRepository ??= new TaskRepository(_context);
            }
        }
        public ICheckListRepository CheckListRepository { 
            get 
            {
                return _checkListRepository ??= new CheckListRepository(_context); 
            } 
         }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}