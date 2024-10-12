using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class ReactionsStatus
    {
        public int Id { get; set; }
        public int? PostId {  get; set; }
        public int? CommentId {  get; set; }
        public int HahaCount { get; set; }
        public int LikeCount { get; set; }
        public int AngryCount { get; set; }
        public int LoveCount { get; set; }
        public int TotalReactionsCount { get; set; }

        // Navigation Properties
        public virtual Post Post { get; set; }
        public virtual Comment Comment { get; set; }

        // Helper methhod - NOT MAPPED 
        public void UpdateTotalReactionsCount()
        {
            TotalReactionsCount = HahaCount + LikeCount + AngryCount + LoveCount;
        }
    }
}
