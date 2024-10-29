using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class Chat
    {
        public int Id { get; set; }
        public int Participant1Id { get; set; }
        public int Participant2Id { get; set; }

        // Navigation Properties
        public virtual ICollection<Message> Messages { get; set; }
        public virtual AppUser Participant1 {  get; set; }
        public virtual AppUser Participant2 {  get; set; }
    }
}
