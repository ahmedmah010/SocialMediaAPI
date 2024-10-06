using SocialMediaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.DTOs
{
    public class SentFriendRequestDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string ProfilePic { get; set; }
        //public FriendRequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }

    }
}
