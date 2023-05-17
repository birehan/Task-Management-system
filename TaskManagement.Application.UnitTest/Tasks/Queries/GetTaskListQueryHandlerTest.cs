using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
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

namespace TaskManagement.Application.UnitTest.Tasks.Queries
{
    public class GetTaskListQueryHandlerTest
    {

        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockRepo;
        private readonly GetTaskListQueryHandler _handler;
        public GetTaskListQueryHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _handler = new GetTaskListQueryHandler(_mockRepo.Object, _mapper);

        }


        [Fact]
        public async Task GetTaskList()
        {
            var result = await _handler.Handle(new GetTaskListQuery(), CancellationToken.None);
            result.ShouldBeOfType<Result<List<TaskDto>>>();
            result.Value.Count.ShouldBe(2);
        }
    }
}



