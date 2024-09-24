using SocialMediaAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces.Services
{
    public interface IResponseService
    {
        Task<ResponseDTO> GenerateErrorResponseAsync(IEnumerable<string> errors, string msg= "", object data = default);
        Task<ResponseDTO> GenerateErrorResponseAsync(string error, string msg = "", object data = default);
        Task<ResponseDTO> GenerateSuccessResponseAsync(string msg="", object data = default);
    }
}
