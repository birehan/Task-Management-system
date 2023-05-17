using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.UnitTest.Mocks;
using TaskManagement.Application.UnitTests.Mocks;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.UnitTest.Mocks
{
    public static class MockUnitOfWork
    {
        public static int changes = 0;
        public static Mock<IUnitOfWork> GetUnitOfWork()
        {   var mockUow = new Mock<IUnitOfWork>();
            var mockTaskRepo = MockTaskRepository.GetTaskRepository();
            var mockCheckListRepo = MockCheckListRepository.GetCheckListRepository();


            mockUow.Setup(r => r.TaskRepository).Returns(mockTaskRepo.Object);
            mockUow.Setup(t => t.CheckListRepository).Returns(mockCheckListRepo.Object);


            mockUow.Setup(r => r.Save()).ReturnsAsync(1);
           return mockUow;
    }

     
}
}
