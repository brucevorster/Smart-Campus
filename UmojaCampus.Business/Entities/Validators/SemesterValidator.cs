using FluentValidation;
namespace UmojaCampus.Business.Entities.Validators
{
    public class SemesterValidator : AbstractValidator<Semester>
    {
        public SemesterValidator()
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
