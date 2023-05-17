using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.UnitTest.Mocks;
using TaskManagement.Domain;
using MediatR;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;

namespace TaskManagement.Application.UnitTests.Mocks
{
    public static class MockCheckListRepository
    {
        public static Mock<ICheckListRepository> GetCheckListRepository()
        {
            var CheckLists = new List<CheckList>
            {
                 new CheckList
                {
                    Id = 1,
                    TaskId = 1,
                    Title = "title 1",
                    Description = "Sample Content 1",
                    Status = true,
                },

                new CheckList
                {

                    Id = 2,
                    TaskId = 1,
                    Title = "title 2",
                    Description = "Sample Content 2",
                    Status = true,
                },

            };

            var mockRepo = new Mock<ICheckListRepository>();

            mockRepo.Setup(r => r.GetAll()).ReturnsAsync(CheckLists);

            mockRepo.Setup(r => r.Add(It.IsAny<CheckList>())).ReturnsAsync((CheckList CheckList) =>
            {   
                CheckList.Id = CheckLists.Count() + 1;
                CheckLists.Add(CheckList);
                MockUnitOfWork.changes += 1;
                return CheckList;
            }); 

            mockRepo.Setup(r => r.Update(It.IsAny<CheckList>())).Callback((CheckList CheckList) =>
            {
                var newCheckLists = CheckLists.Where((r) => r.Id != CheckList.Id);
                CheckLists = newCheckLists.ToList();
                CheckLists.Add(CheckList);
                MockUnitOfWork.changes += 1;
            });

            mockRepo.Setup(r => r.Delete(It.IsAny<CheckList>())).Callback((CheckList CheckList) =>
            {
                if (CheckLists.Remove(CheckList))
                    MockUnitOfWork.changes += 1;
            });

            mockRepo.Setup(r => r.Exists(It.IsAny<int>())).ReturnsAsync((int Id) =>
            {
                var CheckList = CheckLists.FirstOrDefault((r) => r.Id == Id);
                return CheckList != null;
            });


            mockRepo.Setup(r => r.Get(It.IsAny<int>())).ReturnsAsync((int Id) =>
            {   
                return CheckLists.FirstOrDefault((r) => r.Id == Id);
            });


            return mockRepo;

        }
    }
}
