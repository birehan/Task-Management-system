using MediatR;
using TaskManagement.Application.Responses;

namespace TaskManagement.Application.Features.Tasks.CQRS.Commands
{
    public class DeleteTaskCommand: IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
}
