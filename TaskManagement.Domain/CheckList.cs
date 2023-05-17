using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Domain.Common;

namespace TaskManagement.Domain
{
    public class CheckList : BaseDomainEntity
    {
    public string Title { get; set; }
    public string Description { get; set; }

    public bool Status { get; set; }

    public int TaskId { get; set; }
    
    public Task Task { get; set; } 


    }
}