using System.ComponentModel.DataAnnotations;
using UmojaCampus.Business.Entities.Base;
using UmojaCampus.Business.Entities.Validators;

namespace UmojaCampus.Business.Entities
{
    public class Semester: BaseEntity, IValidatableObject
    {
        public string Name { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            var validator = new SemesterValidator();

            var result = validator.Validate(this);

            return result.Errors.Select(item =>
                new ValidationResult(item.ErrorMessage, [item.PropertyName]));
        }
    }
}
