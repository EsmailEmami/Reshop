using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Reshop.Application.Interfaces.User;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reshop.Application.Security.Attribute
{
    // we can give list of permissionName like RoleManager,UserManager,...
    public class PermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _permissionsName;

        private IRoleService _roleService;

        public PermissionAttribute(string permissionsName)
        {
            _permissionsName = permissionsName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _roleService = (IRoleService)context.HttpContext.RequestServices.GetService(typeof(IRoleService));

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();

                if (!_roleService.PermissionChecker(userId, _permissionsName))
                {
                    var refererPath = context.HttpContext.Request.Headers["Referer"].ToString();

                    context.Result = string.IsNullOrEmpty(refererPath) ? new RedirectResult("/") : new RedirectResult(refererPath);
                }
            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }
        }
    }
}
