using FluentValidation;
using SocialMediaAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Validators
{
    public class PostDTOValidator : AbstractValidator<PostDTO>
    {
        public PostDTOValidator() 
        {
            RuleFor(p=>p.Content).NotEmpty().When(p=>p.Photos == null && p.Videos == null).WithMessage("You can't add an empty post");
            RuleFor(p => p.Content).MaximumLength(500).WithMessage("Your post exceeds the length of 500 characters");
        }
    }
}
