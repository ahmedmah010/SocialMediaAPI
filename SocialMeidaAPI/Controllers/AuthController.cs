using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;

namespace SocialMediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterationForm)
        {
            ResponseDTO response = await _accountService.RegisterAsync(userRegisterationForm);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginForm)
        {
            ResponseDTO response = await _accountService.LoginAsync(userLoginForm);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
