using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;
using UmojaCampus.Business.Entities;

namespace UmojaCampus.API.Controllers.Base
{
    public class BaseAuthController : ControllerBase
    {
        public BaseAuthController() { }

        protected CurrentUser GetUser(IIdentity user)
        {
            var userIdentity = GetClaimsIdentity(user) ?? throw new UnauthorizedAccessException();
            var email = userIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var userId = userIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null || email == null)
            {
                throw new UnauthorizedAccessException();
            }

            var role = userIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            return new CurrentUser
            {
                Id = userId,
                Email = email,
                Role = role
            };
        }

        private static ClaimsIdentity GetClaimsIdentity(IIdentity user) 
        {
            return user as ClaimsIdentity;
        }
    }
}
