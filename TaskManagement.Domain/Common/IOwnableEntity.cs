using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Common
{
    public interface IOwnableEntity
    {
   public string CreatorId { get; set; }

    public AppUser Creator { get; set; }
        
    }
}