using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class PostMedia
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int PostId {  get; set; }
        // Navigation Property 
        public virtual Post Post { get; set; }
    }
}
