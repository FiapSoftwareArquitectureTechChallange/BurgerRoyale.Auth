using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Helpers;
using FluentValidation;

namespace BurgerRoyale.Auth.API.Validators
{
    public class UserRegisterRequestDTOValidator : AbstractValidator<CustomerRequestDTO>
    {
        public UserRegisterRequestDTOValidator()
        {
            When(w => w is not null, () =>
            {
                RuleFor(r => r.Cpf)                    
                    .NotEmpty()
                    .Must(x => CpfHelper.IsValid(x))
                    .WithMessage("Informe um CPF válido");

                RuleFor(r => r.Name)
                   .NotEmpty()
                   .WithMessage("Informe um nome");

                RuleFor(r => r.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage("Informe um email válido");

                RuleFor(r => r.Password)
                    .NotEmpty()
                    .WithMessage("Informe uma senha");

                RuleFor(r => r.PasswordConfirmation)
                    .NotEmpty()
                    .Equal(x => x.Password)
                    .WithMessage("Confirmação de senha incorreta");
            });
        }
    }
}
