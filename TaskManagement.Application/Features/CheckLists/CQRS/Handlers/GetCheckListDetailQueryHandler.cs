using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.CheckLists.CQRS.Queries;
using TaskManagement.Application.Features.CheckLists.DTOs;
using TaskManagement.Application.Responses;
using MediatR;

namespace TaskManagement.Application.Features.CheckLists.CQRS.Handlers
{
    public class GetCheckListDetailQueryHandler : IRequestHandler<GetCheckListDetailQuery, Result<CheckListDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCheckListDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CheckListDto>> Handle(GetCheckListDetailQuery request, CancellationToken cancellationToken)
        {
            var response = new Result<CheckListDto>();
            var checkList = await _unitOfWork.CheckListRepository.Get(request.Id);

            if (checkList == null)
            {
                return null;
            }

            response.Success = true;
            response.Message = "Fetch Success";
            response.Value = _mapper.Map<CheckListDto>(checkList);

            return response;
        }
    }
}
