using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.DTOs
{
    public class AddReplyDTO
    {
        [Required]
        public int ParentCommentId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
