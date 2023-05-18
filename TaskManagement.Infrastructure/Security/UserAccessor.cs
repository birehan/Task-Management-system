// using System.Security.Claims;
// using TaskManagement.Application.Interfaces;
// using Microsoft.AspNetCore.Http;
// using TaskManagement.Domain;
// using Microsoft.EntityFrameworkCore;

// namespace TaskManagement.Infrastructure.Security
// {
//     public class UserAccessor : IUserAccessor
//     {
//         private readonly IHttpContextAccessor _httpContextAccessor;

//         private readonly DbContext _dbContext;
//         public UserAccessor(IHttpContextAccessor httpContextAccessor, DbContext dbContext)
//         {
//             _httpContextAccessor = httpContextAccessor;
//             _dbContext = dbContext;
//         }

//         public string GetUsername()
//         {
//             return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
//         }


//         public async Task<AppUser> GetCurrentUser()
//         {
//             var username = GetUsername();
//             return await _dbContext.Set<AppUser>().SingleOrDefaultAsync(u => u.UserName == username);
//         }

     
//     }
// }

using System.Security.Claims;
using TaskManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using TaskManagement.Domain;
using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public UserAccessor(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }

        public async Task<AppUser> GetCurrentUser()
        {
            var username = GetUsername();
            return await _userManager.FindByNameAsync(username);
        }
    }
}
