using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int? ParentCommentId { get; set; }
        public string Content { get; set; }
        public string Media {  get; set; }
        public DateTime Date { get; set; }

        //Navigation Properties
        public virtual AppUser User { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment ParentComment {  get; set; }
        public virtual ICollection<Comment> ChildComments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<CommentReaction> Reactions { get; set; } = new HashSet<CommentReaction>();

    }
}
