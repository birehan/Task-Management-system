using AutoMapper;
using FluentValidation;
using Moq;
using Shouldly;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.Tasks.CQRS.Commands;
using TaskManagement.Application.Features.Tasks.CQRS.Handlers;
using TaskManagement.Application.Features.Tasks.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Responses;
using TaskManagement.Application.UnitTest.Mocks;
using Xunit;
using TaskManagement.Application.Profiles;

namespace TaskManagement.Application.UnitTest.Tasks.Commands
{
    public class CreateTaskCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockRepo;
        private readonly Mock<IUserAccessor> _mockUserAccessor;
        private readonly CreateTaskDto _taskDto;
        private readonly CreateTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            _mockUserAccessor = new Mock<IUserAccessor>();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _taskDto = new CreateTaskDto
            {
                Title = "title",
                Description = "Sample Content",
                Status = true,
                StartDate = DateTime.Now.Date,
                EndDate = DateTime.Now.Date,
            };

            _handler = new CreateTaskCommandHandler(_mockRepo.Object, _mapper, _mockUserAccessor.Object);
        }

        [Fact]
        public async Task CreateTask_ValidData_ReturnsSuccessResult()
        {
            // Arrange
            _mockUserAccessor.Setup(u => u.GetCurrentUser()).ReturnsAsync(new Domain.AppUser());

            // Act
            var result = await _handler.Handle(new CreateTaskCommand { TaskDto = _taskDto }, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<Result<int>>();
            result.Success.ShouldBeTrue();

            var tasks = await _mockRepo.Object.TaskRepository.GetAll();
            tasks.Count.ShouldBe(3);
        }

        [Fact]
        public async Task CreateTask_InvalidStartDate_FailsValidation()
        {
            // Arrange
            _mockUserAccessor.Setup(u => u.GetCurrentUser()).ReturnsAsync(new Domain.AppUser());
            _taskDto.StartDate = DateTime.Now.Date.AddDays(-1); // Set an invalid start date (before the current date)

            // Act
            var result = await _handler.Handle(new CreateTaskCommand { TaskDto = _taskDto }, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<Result<int>>();
            result.Success.ShouldBeFalse();
            result.Errors.ShouldContain("Start Date must be greater than or equal to the current date.");

            var tasks = await _mockRepo.Object.TaskRepository.GetAll();
            tasks.Count.ShouldBe(2); // The task should not be created
        }

        [Fact]
        public async Task CreateTask_InvalidEndDate_FailsValidation()
        {
            // Arrange
            _mockUserAccessor.Setup(u => u.GetCurrentUser()).ReturnsAsync(new Domain.AppUser());
            _taskDto.EndDate = DateTime.Now.Date.AddDays(-1); // Set an invalid end date (before the start date)

            // Act
            var result = await _handler.Handle(new CreateTaskCommand { TaskDto = _taskDto }, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<Result<int>>();
            result.Success.ShouldBeFalse();
            result.Errors.ShouldContain("End Date must be greater than or equal to the Start Date.");

            var tasks = await _mockRepo.Object.TaskRepository.GetAll();
            tasks.Count.ShouldBe(2); // The task should not be created
        }
    }

}
