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
        Task<ResponseDTO> UpdateGenderAsync(string gender);
        Task<ResponseDTO> RemoveGenderAsync();
        //Task<ResponseDTO> UpdateRelationship(string relationship); //Edit it so you can add someone in the RP 
        //Task<ResponseDTO> UpdateEducation(string education); education, languages, workplaces need a speparate table.
        Task<ResponseDTO> GetFriendsAsync(int size, int pageNo);
        Task GetPostsAsync(); // same as above
        Task<ResponseDTO> RemoveFriendAsync(string friendUsername); //May be added to another service (Friendship) or a repo like above
    }
}
