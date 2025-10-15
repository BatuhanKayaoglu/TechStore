using EksiSozluk.Common.ViewModels.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.User.Login
{
    // NOT: Program.cs'e 'AddFluentValidation()' eklemeyi unutma.
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(i => i.EmailAdress).NotNull()
                                       .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible) // .NET'in kendi validator'ı ile yapacak.
                                       .WithMessage("{PropertyName} not a valid email adress");

            RuleFor(i => i.Password).NotNull()
                                   .MinimumLength(6)
                                   .WithMessage("{PropertyName} should at least be {MinLength} characters.");




        }
    }
}
