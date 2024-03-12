using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Helpers;
using FluentValidation;

namespace BurgerRoyale.Auth.Domain.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            When(u => u is not null, () =>
            {
                RuleFor(r => r.Cpf)
                    .NotNull()
                    .NotEmpty()
                    .Must(x => CpfHelper.IsValid(x))
                    .WithMessage("Inform a valid CPF");

                RuleFor(r => r.Name)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Inform a name");

                RuleFor(r => r.Email)
                    .NotNull()
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage("Inform a valid e-mail");

                RuleFor(r => r.PasswordHash)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Inform a password");

                RuleFor(r => r.UserRole)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Inform an user role");
            });
        }
    }
}
