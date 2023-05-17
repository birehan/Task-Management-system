using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.Tasks.CQRS.Handlers;
using TaskManagement.Application.Features.Tasks.CQRS.Commands;
using TaskManagement.Application.Features.Tasks.DTOs;
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
using System.Threading.Tasks;

namespace TaskManagement.Application.UnitTest.Tasks.Commands
{

    public class DeleteTaskCommandHandlerTest
    {
        private readonly Mock<IUnitOfWork> _mockRepo;
        private int _id { get; set; }
        private readonly DeleteTaskCommandHandler _handler;
        public DeleteTaskCommandHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
          
            _id = 1;

            _handler = new DeleteTaskCommandHandler(_mockRepo.Object);

        }


        [Fact]
        public async Task DeleteTask()
        {

            var result = await _handler.Handle(new DeleteTaskCommand() { Id = _id }, CancellationToken.None);
            var Tasksts = await _mockRepo.Object.TaskRepository.GetAll();

            result.ShouldBeOfType<Result<int>>();
            result.Success.ShouldBeTrue();

            var Tasks = await _mockRepo.Object.TaskRepository.GetAll();
            Tasks.Count().ShouldBe(1);
        }

        [Fact]
        public async Task Delete_Task_Doesnt_Exist()
        {

            _id  = 0;
            var result = await _handler.Handle(new DeleteTaskCommand() { Id = _id }, CancellationToken.None);
            result.ShouldBeOfType<Result<int>>();
            result.Success.ShouldBeFalse();
            var Tasks = await _mockRepo.Object.TaskRepository.GetAll();
            Tasks.Count.ShouldBe(2);

        }
    }
}



