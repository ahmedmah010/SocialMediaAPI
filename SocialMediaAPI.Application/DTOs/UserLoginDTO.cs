using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.DTOs
{
    public class UserLoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
