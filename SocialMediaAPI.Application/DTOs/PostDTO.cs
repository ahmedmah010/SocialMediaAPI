using Microsoft.AspNetCore.Http;
using SocialMediaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string? Content {  get; set; }
        public PostPrivacy Privacy {  get; set; }
        public List<IFormFile>? Photos { get; set; }
        public List<IFormFile>? Videos {  get; set; }


    }
}
