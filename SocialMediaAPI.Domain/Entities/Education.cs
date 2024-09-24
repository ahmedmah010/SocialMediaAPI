using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class Education
    {
        public int Id { get; set; }
        public string SchoolName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int ProfileId { get; set; }
        // Navigation Properties 
        public UserProfile Profile { get; set; }
    }
}
