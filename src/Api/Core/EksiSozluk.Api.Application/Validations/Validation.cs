using EksiSozluk.Common.ViewModels.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Validations
{
    public class EntryValidations : AbstractValidator<CreateEntryCommand>
    {
        public EntryValidations()
        {
            RuleFor(x => x.Content).NotEmpty()
                .WithMessage("Content Boş Geçilemez")
                .NotNull()
                .WithMessage("Boş geçilemez")
                .MaximumLength(15)
                .MinimumLength(3)
                .WithMessage("3-15 değer aralığında bir content bilgisi giriniz.");
            RuleFor(x => x.Subject)
                .NotEmpty()
                .WithMessage("Subject Boş Geçilemez")
                .NotNull()
                .WithMessage("Boş geçilemez")
                .MaximumLength(15)
                .MinimumLength(3)
                .WithMessage("3-15 değer aralığında bir subject bilgisi giriniz");
        }

        public class CreateUserValidator : AbstractValidator<CreateUserCommand>
        {
            public CreateUserValidator()
            {
                RuleFor(x => x.EmailAddress).NotEmpty()
                    .WithMessage("EmailAddress Boş Geçilemez")
                    .NotNull()
                    .WithMessage("Boş geçilemez")
                    .EmailAddress().WithMessage("Email adresi doğru formatta değil.");
                RuleFor(x => x.Password)
                    .NotEmpty()
                    .WithMessage("Password Boş Geçilemez")
                    .NotNull()
                    .WithMessage("Password Boş geçilemez")
                    .MaximumLength(15)
                    .MinimumLength(3)
                    .WithMessage("3-15 değer aralığında bir subject bilgisi giriniz");
            }
        }
    }
}
