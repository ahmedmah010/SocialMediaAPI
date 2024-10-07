using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.Mappers;
using SocialMediaAPI.Application.Services;
using SocialMediaAPI.Application.Validators;
using FluentValidation;
using SocialMediaAPI.Application.Interfaces.Services;

namespace SocialMediaAPI.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService,AccountService>();
            services.AddScoped<IResponseService,ResponseService>();
            services.AddScoped<IProfileService,ProfileService>();
            services.AddScoped<ICurrentUserService,CurrentUserService>();
            services.AddScoped<IFriendShipService,FriendShipService>();
            services.AddScoped<IPostService, PostService>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddValidatorsFromAssemblyContaining<UserRegisterValidator>();
            return services;
        }
    }
}
