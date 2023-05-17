
using MediatR;
using TaskManagement.Application.Features.CheckLists.DTOs;
using TaskManagement.Application.Responses;


namespace TaskManagement.Application.Features.CheckLists.CQRS.Queries
{
    public class GetCheckListsQuery : IRequest<Result<List<CheckListDto>>>

    {
        
    }
}