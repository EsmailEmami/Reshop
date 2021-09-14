using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;

namespace Reshop.Application.Convertors
{
    public class RenderViewToString
    {
        public static string RenderRazorViewToString(Controller controller, string viewName = null, object model = null)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }


            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                if (viewEngine != null)
                {
                    ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                    ViewContext viewContext = new ViewContext(
                        controller.ControllerContext,
                        viewResult.View,
                        controller.ViewData,
                        controller.TempData,
                        sw,
                        new HtmlHelperOptions()
                    );
                    viewResult.View.RenderAsync(viewContext);
                }


                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
