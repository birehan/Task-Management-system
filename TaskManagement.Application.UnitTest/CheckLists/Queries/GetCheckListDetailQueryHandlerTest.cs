using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.CheckLists.CQRS.Commands;
using TaskManagement.Application.Features.CheckLists.CQRS.Handlers;
using TaskManagement.Application.Features.CheckLists.CQRS.Queries;
using TaskManagement.Application.Features.CheckLists.DTOs;
using TaskManagement.Application.Profiles;
using TaskManagement.Application.Responses;
using TaskManagement.Application.UnitTest.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Application.UnitTest.CheckLists.Queries
{
    public class GetCheckListDetailQueryHandlerTest
    {

        private readonly IMapper _mapper;
        private readonly Mock<IUnitOfWork> _mockRepo;
        private int Id;
        private readonly GetCheckListDetailQueryHandler _handler;
        public GetCheckListDetailQueryHandlerTest()
        {
            _mockRepo = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            Id = 1;

            _handler = new GetCheckListDetailQueryHandler(_mockRepo.Object, _mapper);

        }


        [Fact]
        public async Task GetCheckListDetail()
        {
            var result = await _handler.Handle(new GetCheckListDetailQuery() { Id = Id }, CancellationToken.None);
            result.ShouldBeOfType<Result<CheckListDto>>();
             result.Value.ShouldBeOfType<CheckListDto>();
        }

        [Fact]
       public async Task GetNonExistingCheckList()
        {
            // Set a non-existing Id
            var nonExistingId = 0;

            // Invoke the handler with the non-existing Id
            var result = await _handler.Handle(new GetCheckListDetailQuery() { Id = nonExistingId }, CancellationToken.None);

            // Assert that the response is a NotFound result
            result.ShouldBe(null);
        }
    }
}

