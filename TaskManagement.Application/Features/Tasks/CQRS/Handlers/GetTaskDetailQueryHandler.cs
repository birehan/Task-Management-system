using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.Tasks.CQRS.Queries;
using TaskManagement.Application.Features.Tasks.DTOs;
using MediatR;
using TaskManagement.Application.Responses;

namespace TaskManagement.Application.Features.Tasks.CQRS.Handlers
{
    public class GetTaskDetailQueryHandler : IRequestHandler<GetTaskDetailQuery, Result<TaskDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTaskDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<TaskDto>> Handle(GetTaskDetailQuery request, CancellationToken cancellationToken)
        {
            var response = new Result<TaskDto>();
            var task = await _unitOfWork.TaskRepository.Get(request.Id);

            if (task == null) return null;

            response.Success = true;
            response.Message = "Fetch Success";
            response.Value = _mapper.Map<TaskDto>(task);

            return response;
        }
    }
}