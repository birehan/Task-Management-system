using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Domain;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;

namespace TaskManagement.Application.UnitTest.Mocks
{
    public static class MockTaskRepository
    {
        public static Mock<ITaskRepository> GetTaskRepository()
        {
            var Tasks = new List<Domain.Task>
            {
                 new Domain.Task
                {
                     Title = "title",
                    Description = "Sample Content",
                    Status = false,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date,
                    Id = 1
                },

                new Domain.Task
                {
                    Title = "title 2",
                    Description = "Sample Content 2",
                    Status = false,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.Now.Date,
                    Id = 2
                }
            };

            var mockRepo = new Mock<ITaskRepository>();

            mockRepo.Setup(r => r.GetAll()).ReturnsAsync(Tasks);

            mockRepo.Setup(r => r.Add(It.IsAny<Domain.Task>())).ReturnsAsync((Domain.Task Task) =>
            {
                Task.Id = Tasks.Count() + 1;
                Tasks.Add(Task);
                MockUnitOfWork.changes += 1;
                return Task;
            });

            mockRepo.Setup(r => r.Update(It.IsAny<Domain.Task>())).Callback((Domain.Task Task) =>
            {
                var newTasks = Tasks.Where((r) => r.Id != Task.Id);
                Tasks = newTasks.ToList();
                Tasks.Add(Task);
                MockUnitOfWork.changes += 1;
            });

            mockRepo.Setup(r => r.Delete(It.IsAny<Domain.Task>())).Callback((Domain.Task Task) =>
            {
                 if (Tasks.Remove(Task))
                    MockUnitOfWork.changes += 1;
            });

            mockRepo.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync((int Id) =>
            {
                return Tasks.FirstOrDefault((r) => r.Id == Id);
            });


            return mockRepo;

        }
    }
}
