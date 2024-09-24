using SocialMediaAPI.Application.DTOs.Response;
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
        Task<ResponseDTO> UpdateProfilePictureAsync(string picture);
        Task<ResponseDTO> RemoveProfilePictureAsync();
        Task<ResponseDTO> UpdateBioAsync(string bio);
        Task<ResponseDTO> RemoveBioAsync();
        Task<ResponseDTO> UpdateCurrentCityAsync(object city); //replace object with a DTO, this function will be used for both add/update
        Task<ResponseDTO> RemoveCurrentCityAsync(int id);
        Task<ResponseDTO> UpdateFromCityAsync(object city);
        Task<ResponseDTO> RemoveFromCityAsync(int id);
        Task<ResponseDTO> UpdateBirthDateAsync(DateTime date);
        Task<ResponseDTO> RemoveBirthDateAsync();
        Task<ResponseDTO> UpdateGenderAsync(string gender);
        Task<ResponseDTO> RemoveGenderAsync();
        //Task<ResponseDTO> UpdateRelationship(string relationship); //Edit it so you can add someone in the RP 
        //Task<ResponseDTO> UpdateEducation(string education); education, languages, workplaces need a speparate table.
    }
}
