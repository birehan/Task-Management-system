using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.Tasks.CQRS.Commands;
using TaskManagement.Application.Features.Tasks.CQRS.Handlers;
using TaskManagement.Application.Features.Tasks.CQRS.Queries;
using TaskManagement.Application.Features.Tasks.DTOs;
using TaskManagement.Application.Profiles;
using TaskManagement.Application.Responses;
using TaskManagement.Application.UnitTest.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Application.UnitTest.Tasks.Queries
{
    public class GetTaskDetailQueryHandlerTest
    {

        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockRepo;
        private int Id;
        private readonly GetTaskDetailQueryHandler _handler;
        public GetTaskDetailQueryHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            Id = 1;

            _handler = new GetTaskDetailQueryHandler(_mockRepo.Object, _mapper);

        }


        [Fact]
        public async Task GetTaskDetail()
        {
            var result = await _handler.Handle(new GetTaskDetailQuery() { Id = Id }, CancellationToken.None);
            result.ShouldBeOfType<Result<TaskDto>>();
             result.Value.ShouldBeOfType<TaskDto>();
        }

        [Fact]
       public async Task GetNonExistingTask()
        {
            // Set a non-existing Id
            var nonExistingId = 0;

            // Invoke the handler with the non-existing Id
            var result = await _handler.Handle(new GetTaskDetailQuery() { Id = nonExistingId }, CancellationToken.None);

            // Assert that the response is a NotFound result
            result.ShouldBe(null);
        }
    }
}

