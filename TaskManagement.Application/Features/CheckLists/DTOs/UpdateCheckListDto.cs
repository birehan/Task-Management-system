using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Application.Features.Common;

namespace TaskManagement.Application.Features.CheckLists.DTOs
{
    public class UpdateCheckListDto :  BaseDto, ICheckListDto
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public bool Status { get; set; }

    }
}