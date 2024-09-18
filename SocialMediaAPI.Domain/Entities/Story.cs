using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class Story
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public string Media {  get; set; }

        //Navigation Properties
        public virtual AppUser User { get; set; }
        public virtual ICollection<StoryViewer> Viewers { get; set; } = new HashSet<StoryViewer>();

    }
}
