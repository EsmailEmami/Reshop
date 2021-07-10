﻿using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Reshop.Application.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.HttpContext.Request.GetTypedHeaders().Referer == null ||
              context.HttpContext.Request.GetTypedHeaders().Host.Host.ToString() != context.HttpContext.Request.GetTypedHeaders().Referer.Host.ToString())
            {
                var refererPath = context.HttpContext.Request.Headers["Referer"].ToString();

                context.Result = string.IsNullOrEmpty(refererPath) ? new RedirectResult("/") : new RedirectResult(refererPath);
            }
        }
    }
}