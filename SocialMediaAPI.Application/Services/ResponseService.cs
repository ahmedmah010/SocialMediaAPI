using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces;
using SocialMediaAPI.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SocialMediaAPI.Application.Services
{
    public class ResponseService : IResponseService
    {
        public async Task<ResponseDTO> GenerateSuccessResponseAsync(string msg = "", object data = default)
        {
            return new ResponseDTO
            {
                Success = true,
                Message = msg,
                Data = data
            };
        }

        public async Task<ResponseDTO> GenerateErrorResponseAsync(IEnumerable<string> errors, string msg = "", object data = default)
        {
            return new ResponseDTO
            {
                Success = false,
                Message = msg,
                Data = data,
                Errors = errors
            };
        }
        // An overloading to handle a single error string
        public async Task<ResponseDTO> GenerateErrorResponseAsync(string error, string msg = "", object data = default)
        {
            return await GenerateErrorResponseAsync(new List<string> { error }, msg, data);
        }
    }
}
