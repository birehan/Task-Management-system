using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.CheckLists.CQRS.Handlers;
using TaskManagement.Application.Features.CheckLists.CQRS.Commands;
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

    public class DeleteCheckListCommandHandlerTest
    {
        private readonly Mock<IUnitOfWork> _mockRepo;
        private int _id { get; set; }
        private readonly DeleteCheckListCommandHandler _handler;
        public DeleteCheckListCommandHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
          
            _id = 1;

            _handler = new DeleteCheckListCommandHandler(_mockRepo.Object);

        }


        [Fact]
        public async Task DeleteCheckList()
        {

            var result = await _handler.Handle(new DeleteCheckListCommand() { Id = _id }, CancellationToken.None);
            result.ShouldBeOfType<Result<int>>();
            result.Success.ShouldBeTrue();

            var CheckLists = await _mockRepo.Object.CheckListRepository.GetAll();
            CheckLists.Count().ShouldBe(1);
        }

        [Fact]
        public async Task Delete_CheckList_Doesnt_Exist()
        {

            _id  = 0;
            var result = await _handler.Handle(new DeleteCheckListCommand() { Id = _id }, CancellationToken.None);
            result.ShouldBeOfType<Result<int>>();
            result.Success.ShouldBeFalse();
            var CheckLists = await _mockRepo.Object.CheckListRepository.GetAll();
            CheckLists.Count.ShouldBe(2);

        }
    }
}



