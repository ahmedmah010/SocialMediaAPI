using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.DTOs
{
    public class StoryViewDTO
    {
        public int StoryId { get; set; }
        public string UserFullName {  get; set; }
        public string UserProfilePic {  get; set; }
        public string Content { get; set; } = string.Empty;
        public string Media { get; set; } = string.Empty;
        public DateTime DatePosted { get; set; }
    }
}
