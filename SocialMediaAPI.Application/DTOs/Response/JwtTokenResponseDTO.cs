using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.DTOs.Response
{
    public class JwtTokenResponseDTO
    {
        public string Token { get; set; }
        public DateTime Expire {  get; set; }
    }
}
