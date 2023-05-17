using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.Application.Features.CheckLists.DTOs
{
    public interface ICheckListDto
    {
    public string Title { get; set; }
    public string Description { get; set; }

    public bool Status { get; set; }
        
    }
}