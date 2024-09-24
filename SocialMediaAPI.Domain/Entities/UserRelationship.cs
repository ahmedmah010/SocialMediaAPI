using SocialMediaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Entities
{
    public class UserRelationship
    {
        public int Id { get; set; }
        public int RequesterId { get; set; }
        public int PartnerId { get; set; }
        public bool IsConfirmed { get; set; }
        public RelationshipStatus Type { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }

        // Navigation Properties
        public virtual UserProfile Requester { get; set; }
        public virtual UserProfile Partner { get; set; }


    }
}
