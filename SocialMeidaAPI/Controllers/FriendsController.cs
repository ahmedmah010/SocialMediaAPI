using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;

namespace SocialMeidaAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendShipService _friendShipService;
        public FriendsController(IFriendShipService friendShipService)
        {

            _friendShipService = friendShipService;
        }
        [HttpPost("requests/{username}")]
        public async Task<IActionResult> SendFriendRequestAsync([FromRoute]string username)
        {
            ResponseDTO response = await _friendShipService.SendFriendRequestAsync(username);
            return response.Success? Ok(response) : BadRequest(response);
        }
        [HttpPost("requests/{id:int}/accept")]
        public async Task<IActionResult> AcceptFriendRequestAsync([FromRoute] int id)
        {
            ResponseDTO response = await _friendShipService.AcceptFriendRequestAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpDelete("requests/{id:int}/reject")]
        public async Task<IActionResult> RejectFriendRequestAsync([FromRoute] int id)
        {
            ResponseDTO response = await _friendShipService.RejectFriendRequestAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpDelete("requests/{id:int}/revoke")]
        public async Task<IActionResult> RevokeFriendRequestAsync([FromRoute] int id)
        {
            ResponseDTO response = await _friendShipService.RevokeFriendRequestAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet("requests/received")]
        public async Task<IActionResult> GetReceivedFriendRequestsAsync()
        {
            ResponseDTO response = await _friendShipService.GetReceivedFriendRequestsAsync(User.Identity.Name);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet("requests/sent")]
        public async Task<IActionResult> GetSentFriendRequestsAsync()
        {
            ResponseDTO response = await _friendShipService.GetSentFriendRequestsAsync(User.Identity.Name);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllFriendsAsync()
        {
            ResponseDTO response = await _friendShipService.GetAllFriendsAsync(User.Identity.Name);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpDelete("{userId:int}")]
        public async Task<IActionResult> RemoveFriendAsync([FromRoute] int userId)
        {
            ResponseDTO response = await _friendShipService.RemoveFriendAsync(userId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
