using Microsoft.AspNetCore.Identity;
using SocialMediaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfilePic { get; set; } //The name of the picture file stored on server
        public bool IsVerified { get; set; }
        public OnlineStatus OnlineStatus { get; set; }
        public AccountStatus AccountStatus { get; set; }

        //Navigation Properties
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<Story> Stories { get; set; } = new HashSet<Story>();
        public virtual ICollection<FriendShip> Friends { get; set; } = new HashSet<FriendShip>();
        public virtual ICollection<FriendRequest> ReceivedFriendRequests { get; set; }
        public virtual ICollection<FriendRequest> SentFriendRequests { get; set; }
    }
}
