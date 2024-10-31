using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class StoryViewer
    {
        public int StoryId { get; set; }
        public int ViewerId { get; set; }
        public DateTime ViewedAt { get; set; }

        // Navigation Properties
        public virtual AppUser Viewer { get; set; }
        public virtual Story Story { get; set; }
    }
}
