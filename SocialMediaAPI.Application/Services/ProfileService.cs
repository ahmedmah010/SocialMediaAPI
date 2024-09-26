using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Enums;
using SocialMediaAPI.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IRepository<UserProfile> _userProfileRepo;
        private readonly IRepository<WorkPlace> _workplaceRepo;
        private readonly IRepository<Education> _educationRepo;
        private readonly IResponseService _responseService;
        private readonly ICurrentUserService _currentUserService;

        public ProfileService(IRepository<UserProfile> userProfileRepo,
            IResponseService responseService,
            ICurrentUserService currentUserService,
            IRepository<WorkPlace> workplaceRepo,
            IRepository<Education> educationRepo)
        {
            _userProfileRepo = userProfileRepo;
            _responseService = responseService;
            _currentUserService = currentUserService;
            _workplaceRepo = workplaceRepo;
            _educationRepo = educationRepo;
        }

        public Task<ResponseDTO> GetProfileInfoAsync(string username)
        {
            //Profile Photo, friends, posts, location, relationship status, workplaces, education, gender, birthdate, bio.
            
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> RemmoveEducationAsync(int id)
        {
            Education education = await _educationRepo.GetByIdAsync(id);
            if (education != null)
            {
                int currentUserProfileId = await _currentUserService.GetCurrentUserIdAsync();
                if (education.ProfileId == currentUserProfileId)
                {
                    _educationRepo.Remove(education);
                    await _educationRepo.SaveChangesAsync();
                    return await _responseService.GenerateSuccessResponseAsync("education removed");
                }
                return await _responseService.GenerateErrorResponseAsync("Invalid operation");
            }
            return await _responseService.GenerateErrorResponseAsync("education not found");
        }

        public async Task<ResponseDTO> RemoveBioAsync()
        {
            UserProfile currentUserProfile = await _currentUserService.GetCurrentUserProfileAsync();
            currentUserProfile.Bio = null;
            return await _responseService.GenerateSuccessResponseAsync("Bio removed");
        }

        public async Task<ResponseDTO> RemoveCurrentCityAsync()
        {
            UserProfile currentUserProfile = await _currentUserService.GetCurrentUserProfileAsync();
            currentUserProfile.CurrentCity = null;
            return await _responseService.GenerateSuccessResponseAsync("Current city removed");
        }

        public async Task<ResponseDTO> RemoveFromCityAsync()
        {
            UserProfile currentUserProfile = await _currentUserService.GetCurrentUserProfileAsync();
            currentUserProfile.FromCity = null;
            return await _responseService.GenerateSuccessResponseAsync("From city removed");
        }

        public async Task<ResponseDTO> UpdateBioAsync(string bio)
        {
            UserProfile currentUserProfile = await _currentUserService.GetCurrentUserProfileAsync();
            currentUserProfile.Bio = bio;
            return await _responseService.GenerateSuccessResponseAsync("Bio updated");
        }

        public async Task<ResponseDTO> UpdateBirthDateAsync(DateTime date)
        {
            UserProfile currentUserProfile = await _currentUserService.GetCurrentUserProfileAsync();
            currentUserProfile.Birthdate = date;
            return await _responseService.GenerateSuccessResponseAsync("Birthdate updated");
        }

        public async Task<ResponseDTO> UpdateEducationAsync(Education education)
        {
            UserProfile currentUserProfile = await _currentUserService.GetCurrentUserProfileAsync(profile => profile.Educations);
            if (currentUserProfile != null)
            {
                // There should be IValidator for education
                education.ProfileId = currentUserProfile.Id; // To explicitly ensure that this will be added/updated for this current user, and remove the overhead of passing the current user profile id from the frontend 
                ResponseDTO response = await _responseService.GenerateSuccessResponseAsync();
                if (education.Id == 0)
                {
                    await _educationRepo.AddAsync(education);
                    response.Message = "Added new education";
                }
                else
                {
                    await _educationRepo.UpdateAsync(education);
                    response.Message = "Updated education";
                }
                await _userProfileRepo.SaveChangesAsync();
                return response;
            }
            return await _responseService.GenerateErrorResponseAsync("User not found");
        }
        public async Task<ResponseDTO> UpdateCurrentCityAsync(string city)
        {
            UserProfile currentUserProfile = await _currentUserService.GetCurrentUserProfileAsync();
            currentUserProfile.CurrentCity = city;
            return await _responseService.GenerateSuccessResponseAsync("Current city updated");
        }
        public async Task<ResponseDTO> UpdateFromCityAsync(string city)
        {
            UserProfile currentUserProfile = await _currentUserService.GetCurrentUserProfileAsync();
            currentUserProfile.FromCity = city;
            return await _responseService.GenerateSuccessResponseAsync("From city updated");
        }

        public async Task<ResponseDTO> UpdateGenderAsync(Gender gender)
        {
            UserProfile currentUserProfile = await _currentUserService.GetCurrentUserProfileAsync();
            currentUserProfile.Gender = gender;
            return await _responseService.GenerateSuccessResponseAsync("Gender updated");
        }
        public async Task<ResponseDTO> RemmoveWorkPlaceAsync(int id)
        {
            WorkPlace workplace = await _workplaceRepo.GetByIdAsync(id);
            if(workplace != null)
            {
                int currentUserProfileId = await _currentUserService.GetCurrentUserIdAsync();
                if(workplace.ProfileId == currentUserProfileId)
                {
                    _workplaceRepo.Remove(workplace);
                    await _workplaceRepo.SaveChangesAsync();
                    return await _responseService.GenerateSuccessResponseAsync("workplace removed");
                }
                return await _responseService.GenerateErrorResponseAsync("Invalid operation");
            }
            return await _responseService.GenerateErrorResponseAsync("workplace not found");
        }

        public async Task<ResponseDTO> UpdateWorkPlaceAsync(WorkPlace workplace)
        {
            UserProfile currentUserProfile = await _currentUserService.GetCurrentUserProfileAsync(profile=>profile.WorkPlaces);
            if (currentUserProfile != null)
            {
                // There should be IValidator for workplace
                workplace.ProfileId = currentUserProfile.Id; // To explicitly ensure that this will be added/updated for this current user, and remove the overhead of passing the current user profile id from the frontend 
                ResponseDTO response = await _responseService.GenerateSuccessResponseAsync();
                if (workplace.Id == 0)
                {
                    await _workplaceRepo.AddAsync(workplace);
                    response.Message = "Added new workplace";
                }
                else
                {
                    await _workplaceRepo.UpdateAsync(workplace);
                    response.Message = "Updated workplace";
                }
                await _userProfileRepo.SaveChangesAsync();
                return response;
            }
            return await _responseService.GenerateErrorResponseAsync("User not found");
        }
        public async Task<ResponseDTO> RemoveBirthDateAsync()
        {
            UserProfile currentUserProfile = await _currentUserService.GetCurrentUserProfileAsync();
            currentUserProfile.Birthdate = null;
            return await _responseService.GenerateSuccessResponseAsync("Birthdate removed");
        }
        public Task<ResponseDTO> RemoveGenderAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> UpdateProfilePictureAsync(string picture)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseDTO> RemoveProfilePictureAsync()
        {
            throw new NotImplementedException();
        }
        public Task<ResponseDTO> RemoveRelationship()
        {
            throw new NotImplementedException();
        }
        public Task<ResponseDTO> UpdateRelationship(UserRelationship relationship)
        {
            throw new NotImplementedException();
        }
    }
}
