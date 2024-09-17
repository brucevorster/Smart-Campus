using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UmojaCampus.Shared.DTO.Validators;

namespace UmojaCampus.Shared.DTO.Account
{
    public class LoginDto : IValidatableObject
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new LoginDtoValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(x => new ValidationResult(x.ErrorMessage, [x.PropertyName]));
        }
    }
}
