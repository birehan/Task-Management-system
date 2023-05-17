using System.Text;
using TaskManagement.API.Services;
using TaskManagement.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TaskManagement.Persistence;
using TaskManagement.API.DTOs;

namespace TaskManagement.API.Extensions {
    public static class IdentityServerExtensions 
    {

        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(options => 
            {
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;

                // Remove the UserName-related options

               
            })
            .AddEntityFrameworkStores<TaskManagementDbContext>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt => {
                        opt.TokenValidationParameters = new TokenValidationParameters{
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = key,
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                     
             });


            services.AddScoped<TokenService>();
            
            return services;
        }
    }
}


            // services.AddAuthorization(opt => {
            //     opt.AddPolicy("IsActivityHost", policy => {
            //         policy.Requirements.Add(new IsHostRequirement());
            //     });
            // });

            // services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();