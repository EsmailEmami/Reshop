using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Reshop.Application.Security.Attribute;

namespace Reshop.Web.Areas.AdminManagerPanel.Controllers
{
    [Area("AdminManagerPanel")]
    [Route("[controller]")]
    public class FullAdminPanel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
