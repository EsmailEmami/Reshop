using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Application.Interfaces.User;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize]
    public class ShoppersManager : Controller
    {
        private readonly IShopperService _shopperService;

        public ShoppersManager(IShopperService shopperService)
        {
            _shopperService = shopperService;
        }

        [HttpGet]
        public IActionResult Index(int pageId = 1, int take = 24)
        {
            return View(/*_shopperService.GetShoppersInformationWithPagination(pageId, take)*/);
        }
    }
}
