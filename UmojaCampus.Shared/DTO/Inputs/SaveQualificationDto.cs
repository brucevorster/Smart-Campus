using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UmojaCampus.Shared.DTO.Validators;

namespace UmojaCampus.Shared.DTO.Inputs
{
    public class SaveQualificationDto: IValidatableObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? TotalCredit { get; set; }
        public decimal? Fees { get; set; }
        public string CoverImage { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new SaveQualificationDtoValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(x => new ValidationResult(x.ErrorMessage, [x.PropertyName]));
        }
    }
}
