using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;
using SocialMediaAPI.Domain.Enums;

namespace SocialMediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionsController : ControllerBase
    {
        private readonly IReactionService _reactionService;
        private readonly IResponseService _responseService;

        public ReactionsController(IReactionService reactionService, IResponseService responseService)
        {
            _reactionService = reactionService;
            _responseService = responseService;
        }
        [HttpPost("post")]
        public async Task<IActionResult> AddPostReactionAsync(int postId, ReactionType reactionType)
        {
            ResponseDTO response = await _reactionService.AddPostReactionAsync(postId, reactionType);
            return response.Success? Ok(response) : BadRequest(response);
        }
        [HttpPost("comment")]
        public async Task<IActionResult> AddCommentReactionAsync(int commentId, ReactionType reactionType)
        {
            ResponseDTO response = await _reactionService.AddCommentReactionAsync(commentId, reactionType);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoveReactionAsync(int id)
        {
            ResponseDTO response = await _reactionService.RemoveReactionAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet("post")]
        public async Task<IActionResult> GetPostReactionsAsync(int postId)
        {
            ResponseDTO response = await _reactionService.GetPostReactionsAsync(postId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet("comment")]
        public async Task<IActionResult> GetCommenttReactionsAsync(int commentId)
        {
            ResponseDTO response = await _reactionService.GetCommentReactionsAsync(commentId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
