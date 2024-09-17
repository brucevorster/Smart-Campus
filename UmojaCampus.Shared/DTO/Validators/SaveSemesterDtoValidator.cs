using FluentValidation;

using UmojaCampus.Shared.DTO.Inputs;

namespace UmojaCampus.Shared.DTO.Validators
{
    public class SaveSemesterDtoValidator: AbstractValidator<SaveSemesterDto>
    {
        public SaveSemesterDtoValidator()
        {
            RuleFor(x => x.Name)
                  .NotEmpty()
                  .MaximumLength(255);

            RuleFor(x => x.DateFrom)
                .NotEmpty();

            RuleFor(x => x.DateTo)
                .NotEmpty();
        }
    }
}
