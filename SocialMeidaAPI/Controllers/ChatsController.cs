using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Infrastructure.EntityTypeConfiguration;

namespace SocialMediaAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IResponseService _responseService;
        public ChatsController(IMessageService messageService, IResponseService responseService)
        {
            _messageService = messageService;
            _responseService = responseService;
        }
        [HttpPost("{receiverId:int}")]
        public async Task<IActionResult> SendMessageAsync([FromBody]MessageDTO message, [FromRoute]int receiverId)
        {
            ResponseDTO response = await _messageService.SendMessageAsync(message, receiverId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet("{chatId:int}/{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetChatAsync(int chatId, int pageNumber, int pageSize)
        {
            ResponseDTO response = await _messageService.GetChatAsync(chatId, pageNumber, pageSize);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet("{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetChatsAsync(int pageNumber = 1, int pageSize = 5)
        {
            ResponseDTO response = await _messageService.GetChatsAsync(pageNumber, pageSize);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpPut("Message")]
        public async Task<IActionResult> MarkMessageAsSeenAsync([FromBody]List<int> messagesIds)
        {
            ResponseDTO response = await _messageService.MarkMessageAsSeenAsync(messagesIds);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
