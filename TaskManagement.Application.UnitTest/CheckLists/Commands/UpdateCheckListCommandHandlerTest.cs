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
using System.Linq;
using System.Text;

namespace TaskManagement.Application.UnitTest.CheckLists.Commands

{
    public class UpdateCheckListCommandHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockRepo;
        private readonly UpdateCheckListDto _CheckListDto;
        private readonly UpdateCheckListCommandHandler _handler;
        public UpdateCheckListCommandHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _CheckListDto = new UpdateCheckListDto
            {
                Id = 1,
                Title = "title",
                Description = "Sample Content",
                Status = true,
            };

            _handler = new UpdateCheckListCommandHandler(_mockRepo.Object, _mapper);

        }


        [Fact]
        public async Task UpdateCheckList()
        {
            var result = await _handler.Handle(new UpdateCheckListCommand() { CheckListDto = _CheckListDto }, CancellationToken.None);
            result.ShouldBeOfType<Result<Unit>>();
            result.Success.ShouldBeTrue();

            var CheckList = await _mockRepo.Object.CheckListRepository.Get(_CheckListDto.Id);
            CheckList.Title.Equals(_CheckListDto.Title);
            CheckList.Description.Equals(_CheckListDto.Description);
        }

        [Fact]
        public async Task UpdateCheckList_NonExistentCheckList_Fails()
        {
            // Arrange
            _CheckListDto.Id = 999; // Set a non-existent CheckList ID

            // Act
            var result = await _handler.Handle(new UpdateCheckListCommand() { CheckListDto = _CheckListDto }, CancellationToken.None);

            // Assert
            result.ShouldBe(null);
            var CheckList = await _mockRepo.Object.CheckListRepository.Get(_CheckListDto.Id);
            CheckList.ShouldBeNull(); // The CheckList should not be found in the repository
        }

        [Fact]
        public async Task UpdateCheckList_InvalidData_Fails()
        {
            // Arrange
            _CheckListDto.Title = ""; // Set an empty title to make the data invalid

            // Act
            var result = await _handler.Handle(new UpdateCheckListCommand() { CheckListDto = _CheckListDto }, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<Result<Unit>>();
            result.Success.ShouldBeFalse();
            result.Message.ShouldBe("Update Failed");
            result.Errors.ShouldContain("Title is required.");

            var CheckList = await _mockRepo.Object.CheckListRepository.Get(_CheckListDto.Id);
            CheckList.ShouldNotBeNull(); // The CheckList should still exist in the repository
        }



       


    }
}



