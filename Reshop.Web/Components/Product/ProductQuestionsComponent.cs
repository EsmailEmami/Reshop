using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Reshop.Web.Components.Product
{
    public class ProductQuestionsComponent : ViewComponent
    {
        // type = news,buyers,best
        public async Task<IViewComponentResult> InvokeAsync(int productId, int pageId = 1, string type = "news")
        {
            return null;
        }
    }
}
