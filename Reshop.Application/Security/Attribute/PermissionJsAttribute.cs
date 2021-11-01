﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Reshop.Application.Interfaces.User;
using System;
using System.Security.Claims;

namespace Reshop.Application.Security.Attribute
{
    // for using of this attribute our ajax request must have isValid & returnUrl response context


    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class PermissionJsAttribute : System.Attribute, IAuthorizationFilter
    {
        public string PermissionsName { get; set; }

        private IPermissionService _roleService;

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            _roleService = (IPermissionService)context.HttpContext.RequestServices.GetService(typeof(IPermissionService));

            var refererPath = context.HttpContext.Request.Headers["Referer"].ToString();

            var url = new Uri(refererPath);


            if (context.HttpContext.User.Identity != null && context.HttpContext.User.Identity.IsAuthenticated)
            {
                string userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();

                if (!string.IsNullOrEmpty(PermissionsName))
                {
                    if (_roleService != null && !await _roleService.PermissionCheckerAsync(userId, PermissionsName))
                    {
                        context.Result = new JsonResult(new { isValid = false, returnUrl = "/Error/404" });
                    }
                }
            }
            else
            {
                context.Result = new JsonResult(new { isValid = false,returnUrl = $"/Login?returnUrl={url.LocalPath}" });
            }
        }
    }
}