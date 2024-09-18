using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class FriendShip
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }

        //Navigation Properties
        public virtual AppUser User { get; set; }
        public virtual AppUser Friend { get; set; }
    }
}
