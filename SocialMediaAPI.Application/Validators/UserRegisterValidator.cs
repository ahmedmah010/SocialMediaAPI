using FluentValidation;
using SocialMediaAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Validators
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegisterValidator()
        {
            RuleFor(e=>e.Username).NotEmpty().MinimumLength(3).MaximumLength(20);
            RuleFor(e=>e.FirstName).NotEmpty().MinimumLength(3).MaximumLength(20);
            RuleFor(e=>e.LastName).NotEmpty().MinimumLength(3).MaximumLength(20);
            RuleFor(e => e.Email).NotEmpty().EmailAddress();
            RuleFor(e => e.ConfirmEmail).NotEmpty().EmailAddress().Equal(e=>e.Email);
            RuleFor(e => e.Password).NotEmpty().MinimumLength(6).MaximumLength(20);
            RuleFor(e => e.ConfirmPassword).NotEmpty().Equal(e => e.Password);
        }
    }
}
