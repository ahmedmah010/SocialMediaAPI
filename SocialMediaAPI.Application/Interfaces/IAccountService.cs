using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseDTO<string>> LoginAsync(UserLoginDTO userLoginForm);
        Task<ResponseDTO<string>> LogoutAsync();
        Task<ResponseDTO<string>> RegisterAsync(UserRegisterDTO userRegisterationForm);
        Task<ResponseDTO<string>> ResetPasswordAsync();
        Task<ResponseDTO<string>> ChangePasswordAsync();
        Task<ResponseDTO<string>> ChangeEmailAsync();
        Task<ResponseDTO<string>> GoogleRegisterAsync();
        Task<ResponseDTO<string>> ConfirmEmailAsync();
        Task<ResponseDTO<string>> SendOtpAsync();
    }
}
