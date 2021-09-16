using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Enums.Product;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security.Attribute;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Permission;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reshop.Web.Controllers.User
{
    [Authorize]
    public class AccountManagerController : Controller
    {
        #region Constructor

        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IOriginService _stateService;
        private readonly IShopperService _shopperService;
        private readonly IDataProtector _dataProtector;

        public AccountManagerController(IUserService userService, ICartService cartService, IProductService productService, IOriginService stateService, IShopperService shopperService, IDataProtectionProvider dataProtectionProvider)
        {
            _userService = userService;
            _cartService = cartService;
            _productService = productService;
            _stateService = stateService;
            _shopperService = shopperService;
            _dataProtector = dataProtectionProvider.CreateProtector("Reshop.Web.Controllers.User.AccountManagerController",
                new string[] { "AccountManagerController" });
        }

        #endregion

        [Route("Dashboard")]
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userService.GetUserByIdAsync(userId);

            if (user is null)
                return NotFound();


            return View(user);
        }

        [Route("Address")]
        [HttpGet]
        public IActionResult Address()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var addresses = _userService.GetUserAddresses(userId);

            return View(addresses);
        }

        [Route("UnFinallyOrders")]
        [HttpGet]
        public IActionResult UnFinallyOrders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_cartService.GetNotReceivedOrders(userId));
        }

        [Route("FinallyOrders")]
        [HttpGet]
        public IActionResult FinallyOrders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_cartService.GetReceivedOrders(userId));
        }

        [Route("Questions")]
        [HttpGet]
        public IActionResult Questions()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);



            return View(_userService.GetUserQuestionsForShow(userId));
        }

        [Route("Comments")]
        [HttpGet]
        public IActionResult Comments()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_userService.GetUserCommentsForShow(userId));
        }

        [Route("FavoriteProducts")]
        [HttpGet]
        public async Task<IActionResult> FavoriteProducts(string type = "all", string sortBy = "news", int pageId = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.SortBy = sortBy.ToLower();
            ViewBag.Type = type.ToLower();

            return View(await _productService.GetUserFavoriteProductsWithPagination(userId, type, sortBy, pageId, 18));
        }


        [HttpGet]
        [NoDirectAccess]
        public IActionResult NewAddress()
        {
            ViewBag.States = _stateService.GetStates() as IEnumerable<State>;

            return View(new Address());
        }

        [HttpPost]
        
        [NoDirectAccess]
        public async Task<IActionResult> NewAddress(Address model)
        {
            ViewBag.States = _stateService.GetStates() as IEnumerable<State>;
            ViewBag.Cities = _stateService.GetCitiesOfState(model.StateId) as IEnumerable<City>;
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "NewAddress", model) });

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var address = new Address()
            {
                UserId = userId,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                StateId = model.StateId,
                CityId = model.CityId,
                Plaque = model.Plaque,
                PostalCode = model.PostalCode,
                AddressText = model.AddressText
            };

            var result = await _userService.AddUserAddressAsync(address);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "هنگام ثبت ادرس به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "NewAddress", model) });
            }
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditAddress(string addressId)
        {
            if (string.IsNullOrEmpty(addressId))
            {
                return BadRequest();
            }
            var address = await _userService.GetAddressByIdAsync(addressId);


            if (address is null)
            {
                return NotFound();
            }




            address.UserId = _dataProtector.Protect(address.UserId);

            ViewBag.States = _stateService.GetStates() as IEnumerable<State>;
            ViewBag.Cities = _stateService.GetCitiesOfState(address.StateId) as IEnumerable<City>;

            return View(address);
        }

        [HttpPost]
        
        [NoDirectAccess]
        public async Task<IActionResult> EditAddress(Address model)
        {
            ViewBag.States = _stateService.GetStates() as IEnumerable<State>;
            ViewBag.Cities = _stateService.GetCitiesOfState(model.StateId) as IEnumerable<City>;
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "EditAddress", model) });


            try
            {
                model.UserId = _dataProtector.Unprotect(model.UserId);
            }
            catch
            {
                return BadRequest();
            }

            var address = await _userService.GetAddressByIdAsync(model.AddressId);

            if (address is null)
            {
                return NotFound();
            }

            if (!await _userService.IsUserAddressExistAsync(address.AddressId, model.UserId))
            {
                return BadRequest();
            }


            address.FullName = model.FullName;
            address.StateId = model.StateId;
            address.CityId = model.CityId;
            address.Plaque = model.Plaque;
            address.PhoneNumber = model.PhoneNumber;
            address.PostalCode = model.PostalCode;
            address.AddressText = model.AddressText;


            var result = await _userService.EditUserAddressAsync(address);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "هنگام ثبت ادرس به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "EditAddress", model) });
            }
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditUserInformation()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userService.GetUserDataForEditAsync(userId);

            if (user is null)
            {
                return NotFound();
            }

            user.UserId = _dataProtector.Protect(user.UserId);

            return View(user);
        }

        [HttpPost]
        
        [NoDirectAccess]
        public async Task<IActionResult> EditUserInformation(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "EditUserInformation", model) });


            try
            {
                model.UserId = _dataProtector.Unprotect(model.UserId);
            }
            catch
            {
                return RedirectToAction(nameof(Dashboard));
            }

            var user = await _userService.GetUserByIdAsync(model.UserId);

            if (user is null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.NationalCode = model.NationalCode;
            user.Email = model.Email;

            var result = await _userService.EditUserAsync(user);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "هنگام ویرایش اطلاعات به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "EditUserInformation", model) });
            }
        }


        //-------------------------------------- shopper --------------------------------------

        
    }
}
