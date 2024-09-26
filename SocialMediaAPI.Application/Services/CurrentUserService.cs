using Microsoft.AspNetCore.Http;
using SocialMediaAPI.Application.Interfaces.Services;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<AppUser> _appUserRepo;
        private readonly IRepository<UserProfile> _userProfileRepo;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor,
            IRepository<AppUser> appUserRepo,
            IRepository<UserProfile> userProfileRepo)
        {
            _httpContextAccessor = httpContextAccessor;
            _appUserRepo = appUserRepo;
            _userProfileRepo = userProfileRepo;
        }

        public async Task<AppUser> GetCurrentUserAsync(params Expression<Func<AppUser, object>>[] includes)
        {
            AppUser currentUser;
            int currentUserId = await GetCurrentUserIdAsync();
            if (includes != null)
            {
                currentUser = await _appUserRepo.FindWithIncludesAsync(user => user.Id == currentUserId, includes);
            }
            else
            {
                currentUser = await _appUserRepo.GetByIdAsync(currentUserId);
            }
            return currentUser;
        }

        public async Task<UserProfile> GetCurrentUserProfileAsync(params Expression<Func<UserProfile, object>>[] includes)
        {
            UserProfile currentUserProfile;
            int currentUserId = await GetCurrentUserIdAsync();
            if (includes!=null)
            {
                currentUserProfile = await _userProfileRepo.FindWithIncludesAsync(profile=>profile.Id == currentUserId, includes);
            }
            else
            {
                currentUserProfile = await _userProfileRepo.GetByIdAsync(currentUserId);
            }
            return currentUserProfile;
        }

        public string? GetCurrentUserUsername()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }
        public async Task<int> GetCurrentUserIdAsync()
        {
            AppUser currentUser = await GetCurrentUserAsync();
            return currentUser.Id;
        }
        public bool IsCurrentUserAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
