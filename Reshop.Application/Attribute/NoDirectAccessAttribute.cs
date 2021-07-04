using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Reshop.Application.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.GetTypedHeaders().Referer == null ||
                context.HttpContext.Request.Host.ToString() != context.HttpContext.Request.GetTypedHeaders().Referer.Host.ToString())
            {
                string referer = context.HttpContext.Request.GetTypedHeaders().Referer.ToString();

                if (string.IsNullOrEmpty(referer))
                {
                    context.HttpContext.Response.Redirect("/");
                }

                context.HttpContext.Response.Redirect(referer);
            }
        }
    }
}