using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;

namespace SocialMeidaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpPost]
        public async Task<IActionResult> AddPostCommentAsync([FromBody]AddCommentDTO comment)
        {
            ResponseDTO response = await _commentService.AddPostCommentAsync(comment.PostId, comment.Content);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpPost("replies")]
        public async Task<IActionResult> AddReplyToCommentAsync([FromBody]AddReplyDTO reply) 
        {
            ResponseDTO response = await _commentService.AddReplyToCommentAsync(reply.ParentCommentId, reply.Content);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCommentAsync([FromRoute]int id, [FromBody]string content)
        {
            ResponseDTO response = await _commentService.UpdateCommentAsync(id, content);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> RemoveCommentAsync(int id)
        {
            ResponseDTO response = await _commentService.RemoveCommentAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
