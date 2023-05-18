using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.CheckLists.CQRS.Commands;
using TaskManagement.Application.Features.CheckLists.DTOs.Validators;
using TaskManagement.Application.Responses;
using MediatR;
using TaskManagement.Domain;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Application.Features.CheckLists.CQRS.Handlers
{
    public class CreateCheckListCommandHandler : IRequestHandler<CreateCheckListCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IUserAccessor _userAccessor;

        public CreateCheckListCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,  IUserAccessor userAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
             _userAccessor = userAccessor;
        }

        public async Task<Result<int>> Handle(CreateCheckListCommand request, CancellationToken cancellationToken)
        {
            var response = new Result<int>();
            var validator = new CreateCheckListDtoValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(request.CheckListDto);

            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = "Creation Failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();
            }
            else
            {

                var currentUser = await _userAccessor.GetCurrentUser();

                if (currentUser == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }

                var task = await _unitOfWork.TaskRepository.Get(request.CheckListDto.TaskId);
                if (task.CreatorId != currentUser.Id){
                     response.Success = false;
                    response.Message = "Not Authorized";
                    return response;
                }


                var checkList = _mapper.Map<CheckList>(request.CheckListDto);
                checkList.Creator = currentUser;

                checkList = await _unitOfWork.CheckListRepository.Add(checkList);

                if (await _unitOfWork.Save() > 0)
                {
                    response.Success = true;
                    response.Message = "Creation Successful";
                    response.Value = checkList.Id;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Creation Failed";
                    Console.WriteLine("Failed: " + response.Message);
                }
            }

            return response;
        }
    }
}
