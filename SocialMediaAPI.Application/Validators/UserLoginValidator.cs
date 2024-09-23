using FluentValidation;
using SocialMediaAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginValidator() 
        {
            RuleFor(e=>e.Username).NotEmpty();
            RuleFor(e=>e.Password).NotEmpty();
        }
    }
}
