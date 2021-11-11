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
    // for using of this attribute our ajax request must have isValid & returnUrl response context


    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class PermissionJsAttribute : System.Attribute, IAuthorizationFilter
    {
        private readonly string[] _permissionsName;
        private readonly bool _returnUrl = true;
        private IPermissionService _roleService;

        public PermissionJsAttribute(bool returnUrl, params string[] permissions)
        {
            _permissionsName = permissions;
            _returnUrl = returnUrl;
        }

        public PermissionJsAttribute(params string[] permissions)
        {
            _permissionsName = permissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _roleService = (IPermissionService)context.HttpContext.RequestServices.GetService(typeof(IPermissionService));

            var refererPath = context.HttpContext.Request.Headers["Referer"].ToString();

            var url = new Uri(refererPath);


            if (context.HttpContext.User.Identity != null && context.HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();

                if (string.IsNullOrEmpty(userId))
                    context.Result = new RedirectResult("/Error/403");

                if (!_permissionsName.Any())
                    return;


                if (_roleService != null && !_roleService.PermissionCheckerAsync(userId, _permissionsName).Result)
                {
                    if (_returnUrl)
                    {
                        context.Result = new JsonResult(new { isValid = false, returnUrl = "/Error/403" });
                    }
                    else
                    {
                        context.Result = new RedirectResult("/ErrorJs/403");
                    }
                }

            }
            else
            {
                var skipAuthorization = (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>();

                if (skipAuthorization != null)
                    return;

                context.Result = new JsonResult(new { isValid = false, returnUrl = $"/Login?returnUrl={url.LocalPath}" });
            }
        }
    }
}