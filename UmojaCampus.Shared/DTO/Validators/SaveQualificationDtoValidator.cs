using FluentValidation;
using UmojaCampus.Shared.DTO.Inputs;
using UmojaCampus.Shared.Resources;

namespace UmojaCampus.Shared.DTO.Validators
{
    public class SaveQualificationDtoValidator: AbstractValidator<SaveQualificationDto>
    {
        public SaveQualificationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ErrorResource.RequiredField)
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .NotEmpty()
				.WithMessage(ErrorResource.RequiredField);

            RuleFor(x => x.FromDate)
                .NotEmpty()
				.WithMessage(ErrorResource.RequiredField);

            RuleFor(x => x.ToDate)
                .NotEmpty()
				.WithMessage(ErrorResource.RequiredField);

            RuleFor(x => x.TotalCredit)
                .NotEmpty()
                .WithMessage(ErrorResource.RequiredField)
				.GreaterThan(0)
                .WithMessage(ErrorResource.GreaterThanZero);

            RuleFor(x => x.CoverImage)
                .MaximumLength(255);

            RuleFor(x => x.Fees)
				.NotEmpty()
				.WithMessage(ErrorResource.RequiredField)
				.GreaterThan(0)
                .WithMessage(ErrorResource.GreaterThanZero);
        }
    }
}
