using System.ComponentModel.DataAnnotations;
using UmojaCampus.Business.Entities.Base;
using UmojaCampus.Business.Entities.Validators;

namespace UmojaCampus.Business.Entities
{
    public class Qualification: BaseEntity, IValidatableObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Duration { get; set; }
        public int TotalCredit { get; set; }
        public decimal Fees { get; set; }
        public string CoverImage { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            var validator = new QualificationValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item =>
                new ValidationResult(item.ErrorMessage, [item.PropertyName]));
        }
    }
}
