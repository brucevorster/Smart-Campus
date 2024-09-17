using FluentValidation;

namespace UmojaCampus.Business.Entities.Validators
{
    public class QualificationValidator: AbstractValidator<Qualification>
    {
        public QualificationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(225);

            RuleFor(x => x.FromDate)
                .NotEmpty();

            RuleFor(x => x.ToDate)
                .NotEmpty();

            RuleFor(x => x.CoverImage)
                .MaximumLength(255);

            RuleFor(x => x.Duration)
                .MaximumLength(50);

            RuleFor(x => x.Fees)
                .NotEmpty();
        }
    }
}
