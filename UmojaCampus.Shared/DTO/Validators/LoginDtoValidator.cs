using FluentValidation;
using UmojaCampus.Shared.DTO.Account;

namespace UmojaCampus.Shared.DTO.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
