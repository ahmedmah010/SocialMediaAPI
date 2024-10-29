using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.DTOs
{
    public class MessageDTO
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public int MessageId { get; set; }
        public string FullName {  get; set; } = string.Empty;
        public string ProfileImagePath { get; set; } = string.Empty;
        public DateTime SentAt {  get; set; } = DateTime.UtcNow;
        public bool IsSeen { get; set; }

    }
}
