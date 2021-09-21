using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    public class OfficialProductManager : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
