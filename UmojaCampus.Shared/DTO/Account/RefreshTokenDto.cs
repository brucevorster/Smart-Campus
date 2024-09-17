
using System.ComponentModel.DataAnnotations;

namespace UmojaCampus.Shared.DTO.Account
{
    public class RefreshTokenDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
