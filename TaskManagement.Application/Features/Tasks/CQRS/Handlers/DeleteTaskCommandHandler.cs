using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.Tasks.CQRS.Commands;
using TaskManagement.Application.Responses;
using MediatR;


namespace TaskManagement.Application.Features.Tasks.CQRS.Handlers
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTaskCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var response = new Result<int>();

            var task = await _unitOfWork.TaskRepository.Get(request.Id);

            if (task is null)
            {
                response.Success = false;
                response.Message = "Delete Failed";
            }
            else
            {
                await _unitOfWork.TaskRepository.Delete(task);

                if (await _unitOfWork.Save() > 0)
                {
                    response.Success = true;
                    response.Message = "Delete Successful";
                    response.Value = task.Id;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Delete Failed";
                }
            }

            return response;
        }
    }
}
