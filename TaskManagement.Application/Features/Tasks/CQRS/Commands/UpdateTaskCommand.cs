
using MediatR;
using TaskManagement.Application.Features.Tasks.DTOs;
using TaskManagement.Application.Responses;

namespace TaskManagement.Application.Features.Tasks.CQRS.Commands
{
    public class UpdateTaskCommand: IRequest<Result<Unit>>
    {
        public UpdateTaskDto TaskDto { get; set; }

    }
}
