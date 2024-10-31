using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces.Services
{
    public interface IStoryService
    {
        Task<ResponseDTO> AddStoryAsync(StoryDTO story);
        Task<ResponseDTO> GetFriendsStoriesAsync();
        Task<ResponseDTO> ShowStoryAsync(int storyId, int viewerId);
        Task<ResponseDTO> RemoveStoryAsync(int storyId);
        Task<ResponseDTO> RemoveAllStoriesAsync();
    }
}
