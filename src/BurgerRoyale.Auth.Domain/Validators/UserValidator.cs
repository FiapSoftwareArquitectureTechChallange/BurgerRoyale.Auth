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
                    .WithMessage("Informe um CPF válido");

                RuleFor(r => r.Name)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Informe um nome");

                RuleFor(r => r.Email)
                    .NotNull()
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage("Informe um e-mail válido");

                RuleFor(r => r.PasswordHash)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Informe uma senha");

                RuleFor(r => r.UserRole)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Informe um tipo de usuário");
            });
        }
    }
}
