using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<ResponseDTO> LoginAsync(UserLoginDTO userLoginForm);
        Task<ResponseDTO> RegisterAsync(UserRegisterDTO userRegisterationForm);
        Task<ResponseDTO> ResetPasswordAsync();
        Task<ResponseDTO> ChangePasswordAsync();
        Task<ResponseDTO> ChangeEmailAsync();
        Task<ResponseDTO> GoogleRegisterAsync();
        Task<ResponseDTO> ConfirmEmailAsync();
        Task<ResponseDTO> SendOtpAsync();
    }
}
