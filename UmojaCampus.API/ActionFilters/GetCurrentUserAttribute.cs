using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using UmojaCampus.Business.Entities;

namespace UmojaCampus.API.ActionFilters
{
    public class GetCurrentUserAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = context.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            
            if (userId == null || userEmail == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var currentUser = new CurrentUser
            {
                Id = userId,
                Email = userEmail
            };

            context.HttpContext.Items.Add("CurrentUser", currentUser);
            base.OnActionExecuting(context);
        }
    }
}
