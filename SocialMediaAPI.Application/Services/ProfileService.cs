using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository<UserProfile> _userProfileRepo;
        private readonly IRepository<AppUser> _appUserRepo;
        private readonly IResponseService _responseService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProfileService(IRepository<UserProfile> userProfileRepo,
            IRepository<AppUser> appUserRepo,
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IResponseService responseService)
        {
            _userProfileRepo = userProfileRepo;
            _appUserRepo = appUserRepo;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _responseService = responseService;
        }

        private ClaimsPrincipal? GetCurrentUserClaims()
        {
            return _httpContextAccessor.HttpContext?.User;
        }
        private string? GetCurrentUserUsername()
        {
            return GetCurrentUserClaims()?.Identity?.Name;
        }
        public async Task<ResponseDTO> GetFriendsAsync(int size, int pageNo)
        {
            ResponseDTO response;
            AppUser currentUser = await _appUserRepo.QueryBuilder().Where(u => u.UserName == GetCurrentUserUsername()).Include(user => user.Friends).Take(size).Skip(pageNo).FirstOrDefaultAsync();
            if (currentUser != null)
            {
                response = await _responseService.GenerateSuccessResponseAsync(data: currentUser.Friends);
            }
            else
            {
                response = await _responseService.GenerateErrorResponseAsync("Invalid user");
            }
            return response;
        }

        public Task GetPostsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> GetProfileInfoAsync(string username)
        {
            //Profile Photo, friends, posts, location, relationship status, workplaces, education.
            
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> RemoveBioAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> RemoveBirthDateAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> RemoveCurrentCityAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> RemoveFriendAsync(string friendUsername)
        {
            //will be moved to a FriendService
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> RemoveFromCityAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> RemoveGenderAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> RemoveProfilePictureAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> UpdateBioAsync(string bio)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> UpdateBirthDateAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> UpdateCurrentCityAsync(string city)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> UpdateFromCityAsync(string city)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> UpdateGenderAsync(string gender)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> UpdateProfilePictureAsync(string picture)
        {
            throw new NotImplementedException();
        }
    }
}
