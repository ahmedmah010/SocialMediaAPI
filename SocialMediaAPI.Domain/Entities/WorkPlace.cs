using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class WorkPlace
    {
        public int Id { get; set; }  
        public string CompanyName { get; set; }  
        public string Position { get; set; }  
        public DateTime? StartDate { get; set; }  
        public DateTime? EndDate { get; set; }  

        public int ProfileId { get; set; }
        // Navigation Properties 
        public UserProfile Profile { get; set; }  
    }

}
