using SocialMediaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }

        public FriendRequestStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public AppUser Requester { get; set; }
        [JsonIgnore]
        public AppUser Receiver { get; set; }
    }
}
