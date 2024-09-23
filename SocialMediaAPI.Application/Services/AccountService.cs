using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces;
using SocialMediaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IValidator<UserRegisterDTO> _userRegisterDtoValidator;
        private readonly IValidator<UserLoginDTO> _userLoginDtoValidator;
        private readonly JwtDTO _jwt;
        public AccountService(UserManager<AppUser> userManager,
            IMapper mapper,
            IConfiguration configuration,
            IOptions<JwtDTO> jwt,
            IValidator<UserRegisterDTO> userRegisterDtoValidator,
            IValidator<UserLoginDTO> userLoginDtoValidator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _jwt = jwt.Value;
            _userRegisterDtoValidator = userRegisterDtoValidator;
            _userLoginDtoValidator = userLoginDtoValidator;
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

        public async Task<ResponseDTO<string>> RegisterAsync(UserRegisterDTO userRegisterationForm)
        {
            ResponseDTO<string> response = new ResponseDTO<string>();
            var validationResult = await _userRegisterDtoValidator.ValidateAsync(userRegisterationForm);
            if (!validationResult.IsValid)
            {
                foreach(var error in validationResult.Errors)
                {
                    response.Errors.Add(error.ErrorMessage);
                }
            }
            else
            {
                AppUser newUser = _mapper.Map<AppUser>(userRegisterationForm);
                IdentityResult result = await _userManager.CreateAsync(newUser, userRegisterationForm.Password);
                if (result.Succeeded)
                {
                    response.Success = true;
                    response.Message = "Successful Registeration";
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        response.Errors.Add(error.Description);
                    }
                }
            }
            return response;
        }

        public async Task<ResponseDTO<string>> LoginAsync(UserLoginDTO userLoginForm)
        {
            ResponseDTO<string> response = new ResponseDTO<string>();
            var validationResult = await _userLoginDtoValidator.ValidateAsync(userLoginForm);
            if (!validationResult.IsValid)
            {
                foreach(var error in validationResult.Errors)
                {
                    response.Errors.Add(error.ErrorMessage);
                }
            }
            else
            {
                AppUser user = await _userManager.FindByNameAsync(userLoginForm.Username);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, userLoginForm.Password))
                    {
                        JwtSecurityToken jwt = await CreateJwtAsync(user,userLoginForm.RememberMe);
                        response.Success = true;
                        response.Message = "Successful Login";
                        response.Data = new JwtSecurityTokenHandler().WriteToken(jwt);
                    }
                    else
                    {
                        response.Message = "Unsuccessful Login";
                        response.Errors.Add("Wrong password");
                    }
                }
                else
                {
                    response.Message = "Unsuccessful Login";
                    response.Errors.Add("Username doesn't exist");
                }
            }
            return response;
        }

        public Task<ResponseDTO<string>> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<string>> ChangeEmailAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<string>> ChangePasswordAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<string>> ConfirmEmailAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<string>> GoogleRegisterAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<string>> ResetPasswordAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDTO<string>> SendOtpAsync()
        {
            throw new NotImplementedException();
        }
    }
}


