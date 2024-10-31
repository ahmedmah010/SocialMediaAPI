using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.DTOs
{
    public class StoryDTO
    {
        public string Content {  get; set; }
        public IFormFile Media {  get; set; }
    }
}
