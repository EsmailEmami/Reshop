using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Constants;
using Reshop.Application.Security.Attribute;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    [Route("[controller]")]
    public class AdminPanelController : Controller
    {
        [Permission(PermissionsConstants.AdminPanel)]
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
