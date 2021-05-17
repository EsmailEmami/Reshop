using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Reshop.Web.Areas.AdminManagerPanel.Controllers
{
    [Area("AdminManagerPanel")]
    [Authorize]
    [Route("[controller]")]
    public class FullAdminPanel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
