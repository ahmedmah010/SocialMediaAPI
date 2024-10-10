using SocialMediaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; } // PK
        public int UserId { get; set; } //FK
        public DateTime Date { get; set; }
        public PostPrivacy Privacy { get; set; }
        public string? Content { get; set; }
        //Navigation Properties
        public virtual AppUser User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<PostReaction> Reactions { get; set; } = new HashSet<PostReaction>();
        public virtual ICollection<PostMedia> Media { get; set; } = new HashSet<PostMedia>();

    }

}
