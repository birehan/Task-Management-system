using MediatR;
using TaskManagement.Application.Features.CheckLists.DTOs;
using TaskManagement.Application.Responses;

namespace TaskManagement.Application.Features.CheckLists.CQRS.Queries
{
    public class GetCheckListDetailQuery: IRequest<Result<CheckListDto>>
    {
        public int Id { get; set; }
    }
}