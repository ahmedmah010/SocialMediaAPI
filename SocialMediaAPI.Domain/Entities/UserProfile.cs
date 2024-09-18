using SocialMediaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }  //Both PK and FK to the AppUser Entity
        public string? Bio { get; set; }
        public string? CurrentCity { get; set; }
        public string? FromCity { get; set; }
        public DateTime? Birthdate { get; set; }
        public RelationshipStatus? RelationshipStatus { get; set; }  
        public Gender? Gender { get; set; } 
        public ICollection<string> Languages { get; set; } = new HashSet<string>();
        public ICollection<string> Educations { get; set; } = new HashSet<string>();
        public ICollection<string> WorkPlaces { get; set; } = new HashSet<string>();

        //Navigation Properties
        public virtual AppUser User { get; set; }
    }
}
