using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces;

namespace SocialMeidaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public UserAuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterationForm)
        {
            ResponseDTO<string> response = await _accountService.RegisterAsync(userRegisterationForm);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginForm)
        {
            ResponseDTO<string> response = await _accountService.LoginAsync(userLoginForm);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
