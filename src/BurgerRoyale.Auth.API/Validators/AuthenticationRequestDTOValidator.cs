using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Helpers;
using FluentValidation;

namespace BurgerRoyale.Auth.API.Validators
{
    public class AuthenticationRequestDTOValidator : AbstractValidator<AuthenticationRequestDTO>
    {
        public AuthenticationRequestDTOValidator()
        {
            When(w => w is not null, () =>
            {
                RuleFor(r => r.Cpf)                    
                    .NotEmpty()
                    .When(r => string.IsNullOrEmpty(r.Email))
                    .Must(x => CpfHelper.IsValid(x))
                    .When(r => string.IsNullOrEmpty(r.Email))
                    .WithMessage("Informe um CPF válido");

                RuleFor(r => r.Email)
                    .NotEmpty()
                    .When(r => string.IsNullOrEmpty(r.Cpf))
                    .EmailAddress()
                    .When(r => string.IsNullOrEmpty(r.Cpf))
                    .WithMessage("Informe um email válido");

                RuleFor(r => r.Password)
                    .NotEmpty()
                    .WithMessage("Informe uma senha");
            });
        }
    }
}
