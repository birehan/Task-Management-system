

namespace TaskManagement.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository TaskRepository{get;} 

        ICheckListRepository CheckListRepository { get; }
        
        Task<int> Save();

    }
}