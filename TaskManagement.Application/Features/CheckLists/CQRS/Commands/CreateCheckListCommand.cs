using MediatR;
using TaskManagement.Application.Features.CheckLists.DTOs;
using TaskManagement.Application.Responses;

namespace TaskManagement.Application.Features.CheckLists.CQRS.Commands
{
    public class CreateCheckListCommand :  IRequest<Result<int>>
    {
        public CreateCheckListDto CheckListDto { get; set; }
    }
}
