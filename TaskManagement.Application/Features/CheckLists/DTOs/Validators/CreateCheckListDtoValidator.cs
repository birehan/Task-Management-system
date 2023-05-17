
using FluentValidation;
using TaskManagement.Application.Contracts.Persistence;

namespace TaskManagement.Application.Features.CheckLists.DTOs.Validators
{
    public class CreateCheckListDtoValidator: AbstractValidator<CreateCheckListDto>
    {

        private readonly IUnitOfWork _unitOfWork;


        public CreateCheckListDtoValidator(IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;


            Include(new ICheckListDtoValidator());

              RuleFor(p => p.TaskId)
                    .GreaterThan(0)
                    .MustAsync(async (id, token) => {
                          return  await _unitOfWork.TaskRepository.Exists(id);
                          })
                    .WithMessage("{PropertyName} does't exist");
        }
    }
}