using TaskManagement.Domain;

namespace TaskManagement.Application.Interfaces
{
       public interface IUserAccessor
    {
         string GetUsername();

         Task<AppUser>  GetCurrentUser();

    }
}