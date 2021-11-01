using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Reshop.Application.Interfaces.User;
using System.Security.Claims;

namespace Reshop.Application.Security.Attribute
{
    // we can give list of permissionName like RoleManager,UserManager,...
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string PermissionsName { get; set; }

        private IPermissionService _roleService;


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _roleService = (IPermissionService)context.HttpContext.RequestServices.GetService(typeof(IPermissionService));

            if (context.HttpContext.User.Identity != null && context.HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();


                if (!string.IsNullOrEmpty(PermissionsName))
                {
                    if (_roleService != null && !_roleService.PermissionChecker(userId, PermissionsName))
                    {
                        context.Result = new RedirectResult("/Error/404");
                    }
                }
            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }
        }
    }
}