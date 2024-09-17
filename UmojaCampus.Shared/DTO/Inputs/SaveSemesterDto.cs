using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UmojaCampus.Shared.DTO.Validators;

namespace UmojaCampus.Shared.DTO.Inputs
{
    public class SaveSemesterDto: IValidatableObject
    {
        public string Name { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            // Instantiate a new validator for the current class (SaveSemesterDtoValidator)
            var validator = new SaveSemesterDtoValidator();

            // Validate the current instance using the validator
            var result = validator.Validate(this);

            // Transform the validation errors into a collection of ValidationResult objects
            return result.Errors.Select(item =>
                new ValidationResult(item.ErrorMessage, [item.PropertyName]));
        }
    }
}
