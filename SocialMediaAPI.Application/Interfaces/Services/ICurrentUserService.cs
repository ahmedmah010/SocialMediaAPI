using SocialMediaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        string? GetCurrentUserUsername();
        Task<int> GetCurrentUserIdAsync();
        Task<AppUser> GetCurrentUserAsync(params Expression<Func<AppUser, object>>[] includes);
        Task<UserProfile> GetCurrentUserProfileAsync(params Expression<Func<UserProfile, object>>[] includes);
        bool IsCurrentUserAuthenticated();


    }
}
