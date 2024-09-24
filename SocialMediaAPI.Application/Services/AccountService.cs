using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;
using SocialMediaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SocialMediaAPI.Application.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IValidator<UserRegisterDTO> _userRegisterDtoValidator;
        private readonly IValidator<UserLoginDTO> _userLoginDtoValidator;
        private readonly IResponseService _responseService;
        private readonly JwtDTO _jwt;
        public AccountService(UserManager<AppUser> userManager,
            IMapper mapper,
            IConfiguration configuration,
            IOptions<JwtDTO> jwt,
            IValidator<UserRegisterDTO> userRegisterDtoValidator,
            IValidator<UserLoginDTO> userLoginDtoValidator,
            IResponseService responseService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _jwt = jwt.Value;
            _userRegisterDtoValidator = userRegisterDtoValidator;
            _userLoginDtoValidator = userLoginDtoValidator;
            _responseService = responseService;
        }

        private async Task<JwtSecurityToken> CreateJwtAsync(AppUser user, bool RememberMe)
        {
            DateTime expiration = RememberMe ? DateTime.Now.AddDays(10) : DateTime.Now.AddMinutes(30);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: expiration
            );
            return jwt;
        }
        private async Task<bool> CheckUsernameExistsAsync(string username)
        {
            AppUser user = await _userManager.FindByNameAsync(username);
            return user is not null ? true : false;
        }

        public async Task<ResponseDTO> RegisterAsync(UserRegisterDTO userRegisterationForm)
        {
            ResponseDTO response = new ResponseDTO();
            var validationResult = await _userRegisterDtoValidator.ValidateAsync(userRegisterationForm);
            if (!validationResult.IsValid)
            {
                response = await _responseService.GenerateErrorResponseAsync(validationResult.Errors.Select(error => error.ErrorMessage));
            }
            else if(await CheckUsernameExistsAsync(userRegisterationForm.Username))
            {
                response = await _responseService.GenerateErrorResponseAsync("Username exists");
            }
            else
            { 
                AppUser newUser = _mapper.Map<AppUser>(userRegisterationForm);
                IdentityResult result = await _userManager.CreateAsync(newUser, userRegisterationForm.Password);
                if (result.Succeeded)
                {
                    response = await _responseService.GenerateSuccessResponseAsync();
                }
                else
                {
                    response = await _responseService.GenerateErrorResponseAsync(result.Errors.Select(error => error.Description));
                }
            }
            response.Message = response.Success ? "Successful Registeration" : "Unsuccessful Registeration";
            return response;
        }

        public async Task<ResponseDTO> LoginAsync(UserLoginDTO userLoginForm)
        {
            ResponseDTO response;
            var validationResult = await _userLoginDtoValidator.ValidateAsync(userLoginForm);
            if (!validationResult.IsValid)
            {
                response = await _responseService.GenerateErrorResponseAsync(validationResult.Errors.Select(error => error.ErrorMessage));
            }
            else
            {
                AppUser user = await _userManager.FindByNameAsync(userLoginForm.Username);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, userLoginForm.Password))
                    {
                        JwtSecurityToken jwt = await CreateJwtAsync(user,userLoginForm.RememberMe);
                        JwtTokenResponseDTO data = new JwtTokenResponseDTO { Token = new JwtSecurityTokenHandler().WriteToken(jwt), Expire = jwt.ValidTo };
                        response = await _responseService.GenerateSuccessResponseAsync(data:data);
                    }
                    else
                    {
                        response = await _responseService.GenerateErrorResponseAsync("Wrong password");
                    }
                }
                else
                {
                    response = await _responseService.GenerateErrorResponseAsync("Username doesn't exist");
                }
            }
            response.Message = response.Success ? "Successful Login" : "Unsuccessful Login";
            return response;
        }

        public Task<ResponseDTO> ChangeEmailAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> ChangePasswordAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> ConfirmEmailAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> GoogleRegisterAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> ResetPasswordAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO> SendOtpAsync()
        {
            throw new NotImplementedException();
        }
    }
}


