using FluentValidation;
using SocialMediaAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Validators
{
    public class StoryDTOValidator : AbstractValidator<StoryDTO>
    {
        public StoryDTOValidator() 
        {
            //RuleFor(s => s.Content).NotEmpty().When(s => s.Media == null).WithMessage("You can't add an empty story");
            RuleFor(s => s).Must(s => !string.IsNullOrEmpty(s.Content) || s.Media != null).WithMessage("You can't add an empty story");
            RuleFor(s=>s).Must(s=> !string.IsNullOrEmpty(s.Content) ^ s.Media!=null).WithMessage("You must provide either content or media");

        }
    }
}
