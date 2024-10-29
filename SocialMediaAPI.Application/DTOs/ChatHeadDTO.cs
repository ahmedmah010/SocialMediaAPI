using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.DTOs
{
    public class ChatHeadDTO
    {
        public int ChatId { get; set; }
        public string ReceiverFullName { get; set; }
        public string ReceiverProfilePicPath { get; set; }
        public string LastMsg { get; set; }
        public DateTime LastMsgDate { get; set; }
    }
}
