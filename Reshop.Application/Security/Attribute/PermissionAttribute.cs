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
        private readonly string[] _permissionsName;

        private IRoleService _roleService;

        public PermissionAttribute(string permissionsName)
        {
            _permissionsName = permissionsName.Split(",");
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _roleService = (IRoleService)context.HttpContext.RequestServices.GetService(typeof(IRoleService));

            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();


                var userRolesId = _roleService.GetUserRolesIdByUserId(userId) as IEnumerable<string>;

                if (userRolesId is null)
                    context.Result = new RedirectResult("/Login");

                List<string> permissionsRolesId = new List<string>();


                foreach (var t in _permissionsName)
                {
                    var roles = (IEnumerable<string>)_roleService.GetPermissionRolesIdByPermission(t);

                    foreach (var role in roles)
                    {
                        permissionsRolesId.Add(role);
                    }
                }

                bool isValid = permissionsRolesId.Any(c => userRolesId.Contains(c));

                var refererPath = context.HttpContext.Request.Headers["Referer"].ToString();


                if (isValid == false)
                {
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
