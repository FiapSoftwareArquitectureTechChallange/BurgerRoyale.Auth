using BurgerRoyale.Auth.Domain.DTO;
using FluentValidation;

namespace BurgerRoyale.Auth.API.Validators
{
    public class CustomerUpdateRequestDTOValidator : AbstractValidator<AccountUpdateRequestDTO>
    {
        public CustomerUpdateRequestDTOValidator()
        {
            When(w => w is not null, () =>
            {
                RuleFor(r => r.Name)
                   .NotEmpty()
                   .WithMessage("Informe um nome");

                RuleFor(r => r.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage("Informe um email válido");

                RuleFor(r => r.CurrentPassword)
                    .NotEmpty()
                    .WithMessage("Informe a senha atual");

                RuleFor(r => r.NewPassword)
                    .NotEmpty()
                    .WithMessage("Informe a nova senha");

                RuleFor(r => r.NewPasswordConfirmation)
                    .NotEmpty()
                    .Equal(x => x.NewPassword)
                    .WithMessage("Confirmação de senha incorreta");
            });
        }
    }
}
