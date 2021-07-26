
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
        private readonly IStateService _stateService;
        private readonly IShopperService _shopperService;
        private readonly IDataProtector _dataProtector;

        public AccountManagerController(IUserService userService, ICartService cartService, IProductService productService, IStateService stateService, IShopperService shopperService, IDataProtectionProvider dataProtectionProvider)
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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

        [Route("StoreAddress")]
        [HttpGet]
        public async Task<IActionResult> StoreAddress()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(await _shopperService.GetShopperStoreAddressesAsync(userId));
        }


        [Route("ManageProducts")]
        [HttpGet]
        [Permission("Shopper")]
        public async Task<IActionResult> ManageProducts(ProductTypes type = ProductTypes.All, SortTypes sortBy = SortTypes.News, int pageId = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _productService.GetShopperProductsWithPaginationAsync(userId, type, sortBy, pageId, 20));
        }

        [Route("ProductsAccess")]
        [HttpGet]
        [Permission("Shopper")]
        public async Task<IActionResult> ProductsAccess()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            if (shopperId is null)
                return NotFound();

            return View(_shopperService.GetShopperStoreTitlesName(shopperId));
        }

        [HttpGet]
        [Route("ShopperProductDetail/{productId}")]
        [Permission("Shopper")]
        public async Task<IActionResult> ShopperProductDetail(int productId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            return View(await _productService.GetShopperProductAsync(productId, shopperId));
        }


        // edit color information
        [HttpGet]
        [Permission("Shopper")]
        [NoDirectAccess]
        public async Task<IActionResult> EditProductOfShopper(int productId, int colorId)
        {
            if (productId == 0 && colorId == 0)
                return BadRequest();

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (shopperId is null)
                return NotFound();


            var model = await _productService.GetShopperProductForEditAsync(productId, shopperId, colorId);

            if (model == null)
                return NotFound();


            model.ProductId = _dataProtector.Protect(model.ProductId);
            model.ShopperProductColorId = _dataProtector.Protect(model.ShopperProductColorId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Shopper")]
        [NoDirectAccess]
        public async Task<IActionResult> EditProductOfShopper(EditProductOfShopperViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddShopperToProduct", model) });

            try
            {
                model.ProductId = _dataProtector.Unprotect(model.ProductId);

                model.ShopperProductColorId = _dataProtector.Unprotect(model.ShopperProductColorId);

                // try to convert if is not success means user was changed it
                Convert.ToInt32(model.ProductId);
                Guid.Parse(model.ShopperProductColorId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "EditProductOfShopper", model) });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var shopperProductColor = await _shopperService.GetShopperProductColorAsync(model.ShopperProductColorId);
            if (shopperProductColor is null)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "EditProductOfShopper", model) });
            }

            var shopperProductRequest = new ShopperProductRequest()
            {
                ShopperProductColorId = model.ShopperProductColorId,
                ProductId = Convert.ToInt32(model.ProductId),
                RequestType = false,
                RequestDate = DateTime.Now,
                Price = model.Price,
                QuantityInStock = model.QuantityInStock,
                IsSuccess = false,
                IsRead = false,
                RequestUserId = userId,
            };

            var result = await _shopperService.AddShopperProductRequestAsync(shopperProductRequest);

            if (result == ResultTypes.Successful)
            {
                shopperProductColor.IsActive = model.IsActive;

                var edit = await _shopperService.EditShopperProductColorAsync(shopperProductColor);

                if (edit == ResultTypes.Successful)
                {
                    return Json(new { isValid = true });
                }
            }

            ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "EditProductOfShopper", model) });
        }


        [HttpGet]
        [Permission("Shopper")]
        [NoDirectAccess]
        public async Task<IActionResult> AddProductDiscount(int productId, int colorId)
        {
            if (productId == 0 && colorId == 0)
                return BadRequest();

            var shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (string.IsNullOrEmpty(shopperId))
            {
                return NotFound();
            }

            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);

            if (string.IsNullOrEmpty(shopperProductColorId))
            {
                return NotFound();
            }

            var model = new AddOrEditShopperProductDiscountViewModel()
            {
                ShopperProductColorId = _dataProtector.Protect(shopperProductColorId)
            };

            return View(model);
        }


        [HttpPost]
        [Permission("Shopper")]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> AddProductDiscount(AddOrEditShopperProductDiscountViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddProductDiscount", model) });




            try
            {
                model.ShopperProductColorId = _dataProtector.Unprotect(model.ShopperProductColorId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت تخفیف به مشتکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddProductDiscount", model) });
            }

            DateTime startDate = model.StartDate.ConvertPersianDateTimeToEnglishDateTime();
            DateTime endDate = model.EndDate.ConvertPersianDateTimeToEnglishDateTime();

            if (startDate >= endDate || endDate.Subtract(startDate) < TimeSpan.FromHours(12))
            {
                ModelState.AddModelError("", "فروشنده عزیز تاریخ شروع تخفیف نباید کواه تر از 12 ساعت باشد.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddProductDiscount", model) });
            }



            var shopperProductDiscount = new ShopperProductDiscount()
            {
                ShopperProductColorId = model.ShopperProductColorId,
                StartDate = startDate,
                EndDate = endDate,
                DiscountPercent = model.DiscountPercent,
            };

            var result = await _shopperService.AddShopperProductDiscountAsync(shopperProductDiscount);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }

            ModelState.AddModelError("", "متاسفانه خطایی هنگام ثبت تخفیف رخ داده است. لطفا دوباره تلاش کنید.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "AddProductDiscount", model) });
        }

        [HttpPost]
        [Permission("Shopper")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnAvailableProduct(int productId)
        {
            if (productId == 0)
            {
                return Json(new { isValid = false });
            }


            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (string.IsNullOrEmpty(shopperId))
            {
                return Json(new { isValid = false });
            }

            var result = await _shopperService.UnAvailableShopperProductAsync(shopperId, productId);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true });
            }
            else
            {
                return Json(new { isValid = false });
            }
        }
    }
}
