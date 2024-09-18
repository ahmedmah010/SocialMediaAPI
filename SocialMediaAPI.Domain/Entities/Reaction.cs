using SocialMediaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    //TPH (Table Per Hierarchy) APPROACH
    public abstract class ReactionBase
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public bool IsPostReaction { get; protected set; } //Discriminator
        public ReactionType ReactionType {  get; set; }

        //Navigation Properties
        public virtual AppUser User { get; set; }
    }
    public class PostReaction : ReactionBase
    {
        public PostReaction() => IsPostReaction = true; //Constructor
        public int PostId { get; set; }

        //Navigation Property
        public virtual Post Post { get; set; }
        

    }
    public class CommentReaction : ReactionBase
    {
        public CommentReaction() => IsPostReaction = false; //Constructor
        public int CommentId { get; set; }

        //Navigation Property
        public virtual Comment Comment { get; set; }
    }
}
