
using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.CheckLists.CQRS.Queries;
using TaskManagement.Application.Features.CheckLists.DTOs;
using TaskManagement.Application.Responses;
using MediatR;

namespace TaskManagement.Application.Features.CheckLists.CQRS.Handlers
{
    public class GetCheckListQueryHandler : IRequestHandler<GetCheckListsQuery, Result<List<CheckListDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCheckListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<CheckListDto>>> Handle(GetCheckListsQuery request, CancellationToken cancellationToken)
        {
            var response = new Result<List<CheckListDto>>();
            var checkLists = await _unitOfWork.CheckListRepository.GetAll();
            
            if (checkLists == null)
            {
                return null;
            }

            response.Success = true;
            response.Message = "Fetch Success";
            response.Value = _mapper.Map<List<CheckListDto>>(checkLists);

            return response;
        }
    }
}
