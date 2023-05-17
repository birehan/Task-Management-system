using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.CheckLists.CQRS.Commands;
using TaskManagement.Application.Responses;
using MediatR;

namespace TaskManagement.Application.Features.CheckLists.CQRS.Handlers
{
    public class DeleteCheckListCommandHandler : IRequestHandler<DeleteCheckListCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCheckListCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteCheckListCommand request, CancellationToken cancellationToken)
        {
            var response = new Result<int>();

            var checkList = await _unitOfWork.CheckListRepository.Get(request.Id);

            if (checkList is null)
            {
                response.Success = false;
                response.Message = "Delete Failed";
            }
            else
            {
                await _unitOfWork.CheckListRepository.Delete(checkList);

                if (await _unitOfWork.Save() > 0)
                {
                    response.Success = true;
                    response.Message = "Delete Successful";
                    response.Value = checkList.Id;
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
