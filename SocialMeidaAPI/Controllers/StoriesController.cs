using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;

namespace SocialMediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;
        public StoriesController(IStoryService storyService)
        {
            _storyService = storyService;
        }

        [HttpPost]
        public async Task<IActionResult> AddStoryAsync([FromForm]StoryDTO story)
        {
            ResponseDTO response = await _storyService.AddStoryAsync(story);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet("{storyId:int}")]
        public async Task<IActionResult> ShowStoryAsync(int storyId)
        {
            ResponseDTO response = await _storyService.ShowStoryAsync(storyId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetFriendsStoriesAsync()
        {
            ResponseDTO response = await _storyService.GetFriendsStoriesAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpDelete("{storyId:int}")]
        public async Task<IActionResult> RemoveStoryAsync(int storyId)
        {
            ResponseDTO response = await _storyService.RemoveStoryAsync(storyId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveAllStoriesAsync()
        {
            ResponseDTO response = await _storyService.RemoveAllStoriesAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
