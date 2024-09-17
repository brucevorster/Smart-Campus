using Microsoft.AspNetCore.Identity;

namespace UmojaCampus.Business.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public DateTime? LastLoginDate { get; set; }
        public DateTime CreatedDateTime { get; private set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public string RefreshToken {  get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }    
    }
}
