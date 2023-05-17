using MediatR;
using TaskManagement.Application.Features.CheckLists.DTOs;
using TaskManagement.Application.Responses;

namespace TaskManagement.Application.Features.CheckLists.CQRS.Commands
{
    public class UpdateCheckListCommand: IRequest<Result<Unit>>
    {
        public UpdateCheckListDto CheckListDto { get; set; }
    }
}
