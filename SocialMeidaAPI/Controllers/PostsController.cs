using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;

namespace SocialMediaAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePostAsync([FromForm]PostDTO post)
        {
            ResponseDTO response = await _postService.CreatePostAsync(post);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePostAsync([FromForm] PostDTO post)
        {
            ResponseDTO response = await _postService.UpdatePostAsync(post);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemovePostAsync([FromRoute] int id)
        {
            ResponseDTO response = await _postService.RemovePostAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
