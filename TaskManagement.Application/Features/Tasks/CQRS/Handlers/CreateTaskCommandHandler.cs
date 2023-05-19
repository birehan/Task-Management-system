
using AutoMapper;
using TaskManagement.Application.Contracts.Persistence;
using TaskManagement.Application.Features.Tasks.CQRS.Commands;
using TaskManagement.Application.Features.Tasks.DTOs.Validators;
using TaskManagement.Application.Responses;
using MediatR;
using TaskManagement.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using TaskManagement.Domain;


namespace TaskManagement.Application.Features.Tasks.CQRS.Handlers
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IUserAccessor _userAccessor;



        public CreateTaskCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,  IUserAccessor userAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userAccessor = userAccessor;

        }

        public async Task<Result<int>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {

            var response = new Result<int>();
            var validator = new CreateTaskDtoValidator();
            var validationResult = await validator.ValidateAsync(request.TaskDto);

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

                var task = _mapper.Map<Domain.Task>(request.TaskDto);
                task.Creator = currentUser;

                await _unitOfWork.TaskRepository.Add(task);

                if (await _unitOfWork.Save() > 0)
                {
                    response.Success = true;
                    response.Message = "Creation Successful";
                    response.Value = task.Id;
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
