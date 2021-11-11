using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Reshop.Application.Interfaces.User;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace Reshop.Application.Security.Attribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _permissionsName;
        private IPermissionService _roleService;


        public PermissionAttribute(params string[] permissions)
        {
            _permissionsName = permissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity != null && context.HttpContext.User.Identity.IsAuthenticated)
            {
                _roleService = (IPermissionService)context.HttpContext.RequestServices.GetService(typeof(IPermissionService));

                string userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                    context.Result = new RedirectResult("/Error/403");

                if (!_permissionsName.Any())
                    return;

                if (_roleService != null && !_roleService.PermissionCheckerAsync(userId, _permissionsName).Result)
                {
                    context.Result = new RedirectResult("/Error/403");
                }
            }
            else
            {
                var skipAuthorization = (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>();

                if (skipAuthorization != null)
                    return;

                context.Result = new RedirectResult("/Login");
            }
        }
    }
}