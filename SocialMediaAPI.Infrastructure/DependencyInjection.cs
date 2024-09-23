using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Repositories.Base;
using SocialMediaAPI.Infrastructure.Context;
using SocialMediaAPI.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
            services.AddIdentityCore<AppUser>().AddEntityFrameworkStores<AppDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }
    }
}
