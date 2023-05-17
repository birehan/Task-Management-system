using MediatR;
using TaskManagement.Application.Features.Tasks.DTOs;
using TaskManagement.Application.Responses;

namespace TaskManagement.Application.Features.Tasks.CQRS.Commands
{
    public class CreateTaskCommand: IRequest<Result<int>>
    {
        public CreateTaskDto TaskDto { get; set; }
    }
}
