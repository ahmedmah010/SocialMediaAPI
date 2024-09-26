using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces.Services
{
    public interface IProfileService
    {
        Task<ResponseDTO> GetProfileInfoAsync(string username);
        Task<ResponseDTO> UpdateProfilePictureAsync(string picture); //not string for sure, check how to uplaod files.
        Task<ResponseDTO> RemoveProfilePictureAsync(); 
        Task<ResponseDTO> UpdateBioAsync(string bio);
        Task<ResponseDTO> RemoveBioAsync();
        Task<ResponseDTO> UpdateCurrentCityAsync(string city);
        Task<ResponseDTO> RemoveCurrentCityAsync();
        Task<ResponseDTO> UpdateFromCityAsync(string city);
        Task<ResponseDTO> RemoveFromCityAsync();
        Task<ResponseDTO> UpdateBirthDateAsync(DateTime date);
        Task<ResponseDTO> RemoveBirthDateAsync();
        Task<ResponseDTO> UpdateGenderAsync(Gender gender);
        Task<ResponseDTO> RemoveGenderAsync();
        Task<ResponseDTO> UpdateEducationAsync(Education education);
        Task<ResponseDTO> RemmoveEducationAsync(int id);
        Task<ResponseDTO> UpdateWorkPlaceAsync(WorkPlace workplace);
        Task<ResponseDTO> RemmoveWorkPlaceAsync(int id);
        Task<ResponseDTO> UpdateRelationship(UserRelationship relationship); 
        Task<ResponseDTO> RemoveRelationship(); 
    }
}
