using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.Tasks.CQRS.Queries;
using TaskManagement.Application.Features.Tasks.DTOs;
using TaskManagement.Application.Responses;
using MediatR;

namespace TaskManagement.Application.Features.Tasks.CQRS.Handlers
{
    public class GetTaskListQueryHandler : IRequestHandler<GetTaskListQuery, Result<List<TaskDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTaskListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<TaskDto>>> Handle(GetTaskListQuery request, CancellationToken cancellationToken)
        {
            var response = new Result<List<TaskDto>>();
            var tasks = await _unitOfWork.TaskRepository.GetAll();
            
            if (tasks == null) return null;

            response.Success = true;
            response.Message = "Fetch Success";
            response.Value = _mapper.Map<List<TaskDto>>(tasks);

            return response;
        }
    }
}
