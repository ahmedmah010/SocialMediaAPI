using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.DTOs.Response
{
    public class ResponseDTO<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public T? Data { get; set; }
    }
}
