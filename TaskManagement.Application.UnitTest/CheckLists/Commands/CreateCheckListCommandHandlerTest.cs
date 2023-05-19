using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.CheckLists.CQRS.Commands;
using TaskManagement.Application.Features.CheckLists.CQRS.Handlers;
using TaskManagement.Application.Features.CheckLists.DTOs;
using TaskManagement.Application.Profiles;
using TaskManagement.Application.Responses;
using TaskManagement.Application.UnitTest.Mocks;
using TaskManagement.Application.Interfaces;
using MediatR;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


using Xunit;
namespace TaskManagement.Application.UnitTest.CheckLists.Commands
{
    public class CreateCheckListCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockRepo;
        private readonly CreateCheckListDto _checkListDto;
        private readonly CreateCheckListCommandHandler _handler;

        private readonly Mock<IUserAccessor> _mockUserAccessor;


        public CreateCheckListCommandHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            _mockUserAccessor = new Mock<IUserAccessor>();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _checkListDto = new CreateCheckListDto
            {
                Title = "title",
                Description = "Sample Content",
                Status = true,
                TaskId = 1,
            };

            _handler = new CreateCheckListCommandHandler(_mockRepo.Object, _mapper, _mockUserAccessor.Object);
        }

        [Fact]
        public async Task CreateCheckList_ValidTaskId_ReturnsSuccess()
        {
            // Arrange
            var task = new Domain.Task { Id = _checkListDto.TaskId /* Set other properties accordingly */ };
            _mockUserAccessor.Setup(u => u.GetCurrentUser()).ReturnsAsync(new Domain.AppUser{Id = "1"});


              _mockRepo.Setup(r => r.TaskRepository.Get(_checkListDto.TaskId)).ReturnsAsync((int Id) =>
                {
                return new Domain.Task{Id=_checkListDto.TaskId, CreatorId="1"};
                });



            _mockRepo.Setup(r => r.TaskRepository.Exists(_checkListDto.TaskId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(new CreateCheckListCommand { CheckListDto = _checkListDto }, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<Result<int>>();
            Console.WriteLine(result.Message);
            result.Success.ShouldBeTrue();
            var checkLists = await _mockRepo.Object.CheckListRepository.GetAll();
            checkLists.Count.ShouldBe(3);
        }

        [Fact]
        public async Task CreateCheckList_InvalidTaskId_ReturnsFailure()
        {
            // Arrange
            _checkListDto.TaskId = 999;

            _mockRepo.Setup(r => r.TaskRepository.Exists(_checkListDto.TaskId))
                .ReturnsAsync(false);
            
            _mockUserAccessor.Setup(u => u.GetCurrentUser()).ReturnsAsync(new Domain.AppUser());


            // Act
            var result = await _handler.Handle(new CreateCheckListCommand { CheckListDto = _checkListDto }, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<Result<int>>();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("Creation Failed");
            result.Errors.ShouldNotBeEmpty();
        }
    }
}
