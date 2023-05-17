using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.CheckLists.CQRS.Commands;
using TaskManagement.Application.Features.CheckLists.DTOs.Validators;
using TaskManagement.Application.Responses;
using MediatR;

namespace TaskManagement.Application.Features.CheckLists.CQRS.Handlers
{
    public class UpdateCheckListCommandHandler : IRequestHandler<UpdateCheckListCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCheckListCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(UpdateCheckListCommand request, CancellationToken cancellationToken)
        {
            var response = new Result<Unit>();

            var validator = new UpdateCheckListDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CheckListDto);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Update Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
            }
            else
            {
                var checkList = await _unitOfWork.CheckListRepository.Get(request.CheckListDto.Id);

                if (checkList == null)
                {
                    return null;
                }

                _mapper.Map(request.CheckListDto, checkList);

                await _unitOfWork.CheckListRepository.Update(checkList);

                if (await _unitOfWork.Save() > 0)
                {
                    response.Success = true;
                    response.Message = "Updated Successfully";
                    response.Value = Unit.Value;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Update Failed";
                }
            }

            return response;
        }
    }
}
