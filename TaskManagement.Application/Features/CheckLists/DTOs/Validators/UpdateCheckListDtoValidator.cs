using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace TaskManagement.Application.Features.CheckLists.DTOs.Validators
{
    public class UpdateCheckListDtoValidator: AbstractValidator<UpdateCheckListDto>
    {
        public UpdateCheckListDtoValidator()
        {
            Include(new ICheckListDtoValidator());
            
            RuleFor(p => p.Id).NotNull().WithMessage("{PropertyName} must be present");

        }
    }
}