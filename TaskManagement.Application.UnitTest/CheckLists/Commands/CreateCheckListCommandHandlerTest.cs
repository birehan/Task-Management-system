using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.CheckLists.CQRS.Commands;
using TaskManagement.Application.Features.CheckLists.CQRS.Handlers;
using TaskManagement.Application.Features.CheckLists.DTOs;
using TaskManagement.Application.Profiles;
using TaskManagement.Application.Responses;
using TaskManagement.Application.UnitTest.Mocks;

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
        private readonly CreateCheckListDto _CheckListDto;
        private readonly CreateCheckListCommandHandler _handler;
        public CreateCheckListCommandHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _CheckListDto = new CreateCheckListDto
            {
                Title = "title",
                Description = "Sample Content",
                Status = true,
                TaskId =1,
        };

            _handler = new CreateCheckListCommandHandler(_mockRepo.Object, _mapper);

        }


        [Fact]
        public async Task CreateCheckList()
        {

            // Create a valid task with the specified TaskId
            var task = new Domain.Task { Id = _CheckListDto.TaskId /* Set other properties accordingly */ };

            // Mock the TaskRepository behavior to return the valid task
            _mockRepo.Setup(r => r.TaskRepository.Exists(_CheckListDto.TaskId))
                .ReturnsAsync(true);


             var result = await _handler.Handle(new CreateCheckListCommand { CheckListDto = _CheckListDto }, CancellationToken.None);
            result.ShouldBeOfType<Result<int>>();
            result.Success.ShouldBeTrue();
            var checkLists = await _mockRepo.Object.CheckListRepository.GetAll();
            checkLists.Count.ShouldBe(3);
        }
        [Fact]
    public async Task CreateCheckList_InvalidTaskId_ReturnsFailure()
    {
    // Set an invalid TaskId that doesn't exist
    _CheckListDto.TaskId = 999;

    // Mock the TaskRepository behavior to return false for the task existence
    _mockRepo.Setup(r => r.TaskRepository.Exists(_CheckListDto.TaskId))
        .ReturnsAsync(false);

    var result = await _handler.Handle(new CreateCheckListCommand { CheckListDto = _CheckListDto }, CancellationToken.None);
    result.ShouldBeOfType<Result<int>>();
    result.Success.ShouldBeFalse();
    result.Message.ShouldBe("Creation Failed");
    result.Errors.ShouldNotBeEmpty();
    }

    }
}