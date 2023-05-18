using Microsoft.AspNetCore.Identity;
using System.Collections;

namespace TaskManagement.Domain
{
    public class AppUser : IdentityUser
    {
          public ICollection<Task> Tasks {get; set;}
          public ICollection<CheckList> CheckLists {get; set;}
    }
}