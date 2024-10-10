using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.DTOs
{
    public class AddCommentDTO
    {
        
        public int PostId {  get; set; }
        public string Content { get; set; }
    }
}
