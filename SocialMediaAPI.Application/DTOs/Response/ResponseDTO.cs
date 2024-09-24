using SocialMediaAPI.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.DTOs.Response
{
    public class ResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public object? Data { get; set; }
    }
}
