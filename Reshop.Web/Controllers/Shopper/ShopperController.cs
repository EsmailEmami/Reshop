using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Generator;
using Reshop.Application.Interfaces.Discount;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reshop.Web.Controllers.Shopper
{

    public class ShopperController : Controller
    {
        #region Constructor

        private readonly IProductService _productService;
        private readonly IShopperService _shopperService;
        private readonly IUserService _userService;
        private readonly IOriginService _originService;
        private readonly IPermissionService _permissionService;
        private readonly IDataProtector _dataProtector;
        private readonly IDiscountService _discountService;
        private readonly IBrandService _brandService;

        public ShopperController(IProductService productService, IShopperService shopperService, IUserService userService, IOriginService originService, IDataProtectionProvider dataProtectionProvider, IPermissionService permissionService, IDiscountService discountService, IBrandService brandService)
        {
            _productService = productService;
            _shopperService = shopperService;
            _userService = userService;
            _originService = originService;
            _permissionService = permissionService;
            _discountService = discountService;
            _brandService = brandService;
            _dataProtector = dataProtectionProvider.CreateProtector("Reshop.Web.Controllers.Shopper.ShopperController",
                new string[] { "ShopperController" });
        }

        #endregion

        #region add shopper

        #region Shopper

        [Route("NewShopper")]
        [HttpGet]
        public IActionResult AddShopper()
        {
            var model = new AddShopperViewModel()
            {
                States = _originService.GetStates() as IEnumerable<State>,
                StoreTitles = _shopperService.GetStoreTitles(),
            };


            return View(model);
        }

        [Route("NewShopper")]
        [HttpPost]
        public async Task<IActionResult> AddShopper(AddShopperViewModel model)
        {
            // this is states when page reload states is not null
            model.States = _originService.GetStates() as IEnumerable<State>;
            model.StoreTitles = _shopperService.GetStoreTitles();

            if (!ModelState.IsValid)
                return View(model);



            //// check images is not script

            #region imgValication

            if (!model.OnNationalCardImageName.IsImage() ||
              !model.BackNationalCardImageName.IsImage() ||
              !model.BusinessLicenseImageName.IsImage())
            {
                ModelState.AddModelError("", "فروشنده عزیز لطفا تصاویر خود را درست وارد کنید.");
                return View(model);
            }

            #endregion

            try
            {
                #region user

                var user = new Domain.Entities.User.User
                {
                    FullName = model.FullName,
                    RegisterDate = DateTime.Now,
                    UserAvatar = "userAvatar.jpg",
                    PhoneNumber = model.PhoneNumber,
                    InviteCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                    InviteCount = 0,
                    Score = 0,
                    NationalCode = model.NationalCode,
                    Email = model.Email,
                    IsBlocked = false
                };

                var addUser = await _userService.AddUserAsync(user);

                if (addUser == ResultTypes.Successful)
                {
                    var addUserToRoleResult = await _permissionService.AddUserToRoleAsync(user.UserId, "Shopper");

                    if (addUserToRoleResult != ResultTypes.Successful)
                    {
                        ModelState.AddModelError("", "فروشنده عزیز متاسفانه هنگام ثبت مقام شما به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "فروشنده عزیز متاسفانه هنگام ثبت نام شما به مشکلی غیر منتظره برخورد کردیم. لطفا با پشتیبانی تماس بگیرید.");
                    return View(model);
                }


                #endregion


                var shopper = new Domain.Entities.Shopper.Shopper()
                {
                    StoreName = model.StoreName,
                    BirthDay = model.BirthDay.ConvertPersianDateToEnglishDate(),
                    RegisterShopper = DateTime.Now,
                    IsActive = false,
                    User = user,
                    UserId = user.UserId
                };

                #region shopper cards

                if (model.OnNationalCardImageName.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images",
                        "shoppersCardImages");
                    string imageName = await ImageConvertor.CreateNewImage(model.OnNationalCardImageName, path);
                    shopper.OnNationalCardImageName = imageName;
                }

                if (model.BackNationalCardImageName.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images",
                        "shoppersCardImages");
                    string imageName = await ImageConvertor.CreateNewImage(model.BackNationalCardImageName, path);
                    shopper.BackNationalCardImageName = imageName;
                }

                if (model.BusinessLicenseImageName.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "images",
                        "shoppersCardImages");
                    string imageName = await ImageConvertor.CreateNewImage(model.BusinessLicenseImageName, path);
                    shopper.BusinessLicenseImageName = imageName;
                }

                #endregion

                var addShopper = await _shopperService.AddShopperAsync(shopper);

                if (addShopper == ResultTypes.Successful)
                {

                    #region shopper storeTitle

                    var addStoreTitle = await _shopperService.AddShopperStoreTitleAsync(shopper.ShopperId, model.SelectedStoreTitles as List<int>);

                    if (addStoreTitle != ResultTypes.Successful)
                    {
                        ModelState.AddModelError("", "فروشنده عزیز متاسفانه هنگام ثبت عناوین شما به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                        return View(model);
                    }

                    #endregion

                    #region store address

                    var storeAddress = new StoreAddress()
                    {
                        ShopperId = shopper.ShopperId,
                        CityId = model.CityId,
                        Plaque = model.Plaque,
                        PostalCode = model.PostalCode,
                        AddressText = model.AddressText,
                        LandlinePhoneNumber = model.LandlinePhoneNumber
                    };

                    var addStoreAddress = await _shopperService.AddStoreAddressAsync(storeAddress);

                    if (addStoreAddress == ResultTypes.Successful)
                    {
                        shopper.StoresAddress.Add(storeAddress);

                        await _shopperService.EditShopperAsync(shopper);
                    }
                    else
                    {
                        ModelState.AddModelError("", "فروشنده عزیز متاسفانه هنگام ثبت نشانی فروشگاه شما به مشکلی غیر منتظره برخورد کردیم. لطفا با پشتیبانی تماس بگیرید.");
                        return View(model);
                    }

                    #endregion

                    return Redirect("/");
                }


                ModelState.AddModelError("", "فروشنده عزیز متاسفانه هنگام ثبت نام شما به مشکلی غیر منتظره برخورد کردیم. لطفا با پشتیبانی تماس بگیرید.");
                return View(model);
            }
            catch
            {
                ModelState.AddModelError("", "فروشنده عزیز متاسفانه هنگام ثبت عناوین شما به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return View(model);
            }
        }

        #endregion

        #endregion

        [HttpGet]
        public async Task<IActionResult> ManageProducts()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            return View("ManageProducts", shopperId);
        }

        [HttpGet]
        public async Task<IActionResult> ShopperRequests()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            return View("ShopperRequests", shopperId);
        }

        [HttpGet]
        [NoDirectAccess]
        public IActionResult ShopperProductsList(string shopperId, string type = "all", int pageId = 1, string filter = "")
        {
            return ViewComponent("ShopperProductsForShowShopperComponent", new { shopperId, type, pageId, filter });
        }

        [HttpGet]
        public async Task<IActionResult> StoreAddress()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(await _shopperService.GetShopperStoreAddressesAsync(userId));
        }

        [HttpGet]
        public async Task<IActionResult> ShopperProductDetail(string shopperProductId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            if (shopperId == null)
                return NotFound();

            if (!await _shopperService.IsShopperProductOfShopperAsync(shopperId, shopperProductId))
                return NotFound();

            ViewBag.ShopperProductId = shopperProductId;

            return View(await _productService.GetProductDetailForShopperAsync(shopperProductId));
        }

        // color
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> ShopperProductColorDetail(int productId, int colorId)
        {
            if (productId == 0 && colorId == 0) return NotFound();


            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (shopperId is null)
                return NotFound();

            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);

            if (shopperProductColorId is null)
                return NotFound();

            var model = await _shopperService.GetShopperProductColorDetailAsync(shopperProductColorId);

            if (model is null)
                return NotFound();

            return View(model);
        }

        // discount
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> ShopperProductDiscountDetail(int productId, int colorId)
        {
            if (productId == 0 && colorId == 0) return NotFound();


            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (shopperId is null)
                return NotFound();

            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);

            if (shopperProductColorId is null)
                return NotFound();

            var model = await _discountService.GetShopperProductColorDiscountsGeneralDataAsync(shopperProductColorId);

            if (model is null)
                return NotFound();

            ViewBag.LastDiscount = await _discountService.GetLastShopperProductColorDiscountAsync(shopperProductColorId);

            return View(model);
        }

        #region add or edit shopper product

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddShopperProduct()
        {
            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (shopperId == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var model = new AddShopperProductViewModel()
            {
                ShopperId = shopperId,
            };

            ViewBag.StoreTitles = _shopperService.GetShopperStoreTitles(shopperId);

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddShopperProduct(AddShopperProductViewModel model)
        {
            // data for selectProduct
            ViewBag.StoreTitles = _shopperService.GetShopperStoreTitles(model.ShopperId);
            ViewBag.Brands = _brandService.GetBrandsOfStoreTitle(model.SelectedStoreTitle);
            ViewBag.OfficialProducts = _brandService.GetBrandOfficialProducts(model.SelectedBrand);
            ViewBag.Products = _brandService.GetProductsOfOfficialProduct(model.SelectedOfficialProduct);

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });


            if (await _productService.IsShopperProductExistAsync(model.ShopperId, model.ProductId))
            {
                ModelState.AddModelError("", "این کالا قبلا توسط این شما ثبت شده است. ");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            if (await _shopperService.IsAnyActiveShopperProductRequestExistAsync(model.ShopperId, model.ProductId, true))
            {
                ModelState.AddModelError("", "درخواست شما در حال بررسی است.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shopperProductRequest = new ShopperProductRequest()
            {
                ShopperId = model.ShopperId,
                ProductId = model.ProductId,
                RequestUserId = userId,
                RequestType = true,
                Warranty = model.Warranty,
                RequestDate = DateTime.Now,
                IsSuccess = false,
                IsRead = false,
                IsActive = model.IsActive
            };

            var result = await _shopperService.AddShopperProductRequestAsync(shopperProductRequest);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }

            ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditShopperProduct(string shopperProductId)
        {
            if (shopperProductId == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            if (!await _productService.IsShopperProductExistAsync(shopperProductId))
                return Json(new { isValid = false, errorType = "warning", errorText = "فروشنده یافت نشد! لطفا دوباره تلاش کنید." });

            if (await _shopperService.IsAnyActiveShopperProductRequestExistAsync(shopperProductId, false))
                return Json(new { isValid = false, errorType = "warning", errorText = "درخواست شما در حال بررسی است." });


            var model = await _shopperService.GetShopperProductDataForEditAsync(shopperProductId);

            if (model == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            if (await _shopperService.IsAnyActiveShopperProductRequestExistAsync(model.ShopperId, model.ProductId, false))
                return Json(new { isValid = false, errorType = "danger", errorText = "درخواست شما در حال بررسی است." });

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> EditShopperProduct(EditShopperProductViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            if (!await _productService.IsShopperProductExistAsync(model.ShopperId, model.ProductId))
            {
                ModelState.AddModelError("", "این کالا قبلا توسط این فروشنده ثبت شده است. ");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            if (await _shopperService.IsAnyActiveShopperProductRequestExistAsync(model.ShopperId, model.ProductId, false))
            {
                ModelState.AddModelError("", "درخواست شما در حال بررسی است.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shopperProductRequest = new ShopperProductRequest()
            {
                ShopperId = model.ShopperId,
                ProductId = model.ProductId,
                RequestUserId = userId,
                RequestType = false,
                Warranty = model.Warranty,
                RequestDate = DateTime.Now,
                IsSuccess = false,
                IsRead = false,
                IsActive = model.IsActive
            };

            var result = await _shopperService.AddShopperProductRequestAsync(shopperProductRequest);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }

            ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        #endregion

        #region add shopper product color

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddShopperProductColor(int productId)
        {
            if (productId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (string.IsNullOrEmpty(shopperId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            var shopperProductId = await _shopperService.GetShopperProductIdAsync(shopperId, productId);

            if (string.IsNullOrEmpty(shopperProductId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            var model = new AddColorToShopperProductViewModel()
            {
                ShopperProductId = _dataProtector.Protect(shopperProductId)
            };

            ViewBag.Colors = _shopperService.GetColorsIdAndName();

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddShopperProductColor(AddColorToShopperProductViewModel model)
        {
            ViewBag.Colors = _shopperService.GetColorsIdAndName();
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            try
            {
                model.ShopperProductId = _dataProtector.Unprotect(model.ShopperProductId);

                Guid.Parse(model.ShopperProductId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (string.IsNullOrEmpty(shopperId))
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            if (!await _shopperService.IsShopperProductOfShopperAsync(shopperId, model.ShopperProductId))
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            if (await _shopperService.IsAnyActiveShopperProductColorRequestExistAsync(model.ShopperProductId, model.ColorId, true))
            {
                ModelState.AddModelError("", "درخواست شما در حال بررسی است.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            var shopperProductColorRequest = new ShopperProductColorRequest()
            {
                ShopperProductId = model.ShopperProductId,
                RequestType = true,
                RequestDate = DateTime.Now,
                Price = model.Price,
                QuantityInStock = model.QuantityInStock,
                IsSuccess = false,
                IsRead = false,
                RequestUserId = userId,
                ColorId = model.ColorId,
                IsActive = model.IsActive
            };

            var result = await _shopperService.AddShopperProductColorRequestAsync(shopperProductColorRequest);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "" });
            }

            ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        #endregion

        #region edit shopper product color

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditShopperProductColor(int productId, int colorId)
        {
            if (productId == 0 || colorId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (string.IsNullOrEmpty(shopperId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);

            if (string.IsNullOrEmpty(shopperProductColorId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            var model = await _productService.GetShopperProductColorForEditAsync(shopperProductColorId);

            if (model == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            model.ShopperProductColorId = _dataProtector.Protect(model.ShopperProductColorId);

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> EditShopperProductColor(EditColorOfShopperProductViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            try
            {
                model.ShopperProductColorId = _dataProtector.Unprotect(model.ShopperProductColorId);

                Guid.Parse(model.ShopperProductColorId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // check is user shopper product color
            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            if (string.IsNullOrEmpty(shopperId))
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            if (!await _shopperService.IsShopperProductColorOfShopperAsync(shopperId, model.ShopperProductColorId))
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }


            var shopperProductColor = await _shopperService.GetShopperProductColorAsync(model.ShopperProductColorId);
            if (shopperProductColor == null)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }


            if (await _shopperService.IsAnyActiveShopperProductColorRequestExistAsync(shopperProductColor.ShopperProductId, shopperProductColor.ColorId, false))
            {
                ModelState.AddModelError("", "درخواست شما در حال بررسی است.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            var shopperProductColorRequest = new ShopperProductColorRequest()
            {
                ShopperProductId = shopperProductColor.ShopperProductId,
                ColorId = shopperProductColor.ColorId,
                RequestType = false,
                RequestDate = DateTime.Now,
                Price = model.Price,
                QuantityInStock = model.QuantityInStock,
                IsSuccess = false,
                IsRead = false,
                RequestUserId = userId,
            };

            var result = await _shopperService.AddShopperProductColorRequestAsync(shopperProductColorRequest);

            if (result == ResultTypes.Successful)
            {
                shopperProductColor.IsActive = model.IsActive;

                var edit = await _shopperService.EditShopperProductColorAsync(shopperProductColor);

                if (edit == ResultTypes.Successful)
                {
                    return Json(new { isValid = true, returnUrl = "" });
                }
            }

            ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        #endregion

        #region new discount

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddProductDiscount(int productId, int colorId)
        {
            if (productId == 0 || colorId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (string.IsNullOrEmpty(shopperId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);

            if (string.IsNullOrEmpty(shopperProductColorId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            if (await _discountService.IsActiveShopperProductColorDiscountExistsAsync(shopperProductColorId))
                return Json(new { isValid = false, errorType = "warning", errorText = "فروشنده عزیز شما یک تخفیف فعال دارید." });

            var model = new AddOrEditShopperProductDiscountViewModel()
            {
                ShopperProductColorId = _dataProtector.Protect(shopperProductColorId)
            };

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddProductDiscount(AddOrEditShopperProductDiscountViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            try
            {
                model.ShopperProductColorId = _dataProtector.Unprotect(model.ShopperProductColorId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت تخفیف به مشتکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            if (string.IsNullOrEmpty(shopperId))
            {
                ModelState.AddModelError("", "متاسفانه خطایی هنگام ثبت تخفیف رخ داده است. لطفا دوباره تلاش کنید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            if (!await _shopperService.IsShopperProductColorOfShopperAsync(shopperId, model.ShopperProductColorId))
            {
                ModelState.AddModelError("", "متاسفانه خطایی هنگام ثبت تخفیف رخ داده است. لطفا دوباره تلاش کنید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            if (await _discountService.IsActiveShopperProductColorDiscountExistsAsync(model.ShopperProductColorId))
            {
                ModelState.AddModelError("", "متاسفانه خطایی هنگام ثبت تخفیف رخ داده است. لطفا دوباره تلاش کنید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            DateTime startDate = model.StartDate.ConvertPersianDateTimeToEnglishDateTime();
            DateTime endDate = model.EndDate.ConvertPersianDateTimeToEnglishDateTime();



            if (startDate >= endDate || endDate.Subtract(startDate) < TimeSpan.FromHours(12) ||
                startDate.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("", "فروشنده عزیز تاریخ شروع تخفیف نباید کواه تر از 12 ساعت باشد.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }



            var shopperProductDiscount = new ShopperProductDiscount()
            {
                ShopperProductColorId = model.ShopperProductColorId,
                StartDate = startDate,
                EndDate = endDate,
                DiscountPercent = model.DiscountPercent,
            };

            var result = await _discountService.AddShopperProductDiscountAsync(shopperProductDiscount);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "" });
            }

            ModelState.AddModelError("", "متاسفانه خطایی هنگام ثبت تخفیف رخ داده است. لطفا دوباره تلاش کنید.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        #endregion

        #region edit discount

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditProductDiscount(int productId, int colorId)
        {
            if (productId == 0 || colorId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (string.IsNullOrEmpty(shopperId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperId, productId, colorId);

            if (string.IsNullOrEmpty(shopperProductColorId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });


            if (!await _discountService.IsActiveShopperProductColorDiscountExistsAsync(shopperProductColorId))
                return Json(new { isValid = false, errorType = "warning", errorText = "فروشنده عزیز شما یک تخفیف فعال دارید." });

            var discount = await _discountService.GetLastShopperProductColorDiscountAsync(shopperProductColorId);

            if (discount == null)
                return Json(new { isValid = false, errorType = "warning", errorText = "فروشنده عزیز شما یک تخفیف فعال دارید." });


            var model = new AddOrEditShopperProductDiscountViewModel()
            {
                ShopperProductColorId = _dataProtector.Protect(shopperProductColorId),
                DiscountPercent = discount.DiscountPercent,
                EndDate = discount.EndDate.ToString(),
                StartDate = discount.StartDate.ToString()
            };

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> EditProductDiscount(AddOrEditShopperProductDiscountViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            try
            {
                model.ShopperProductColorId = _dataProtector.Unprotect(model.ShopperProductColorId);
            }
            catch
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت تخفیف به مشتکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            if (string.IsNullOrEmpty(shopperId))
            {
                ModelState.AddModelError("", "متاسفانه خطایی هنگام ثبت تخفیف رخ داده است. لطفا دوباره تلاش کنید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            if (!await _shopperService.IsShopperProductColorOfShopperAsync(shopperId, model.ShopperProductColorId))
            {
                ModelState.AddModelError("", "متاسفانه خطایی هنگام ثبت تخفیف رخ داده است. لطفا دوباره تلاش کنید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            if (!await _discountService.IsActiveShopperProductColorDiscountExistsAsync(model.ShopperProductColorId))
            {
                ModelState.AddModelError("", "متاسفانه خطایی هنگام ثبت تخفیف رخ داده است. لطفا دوباره تلاش کنید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            DateTime startDate = model.StartDate.ConvertPersianDateTimeToEnglishDateTime();
            DateTime endDate = model.EndDate.ConvertPersianDateTimeToEnglishDateTime();



            if (startDate >= endDate || endDate.Subtract(startDate) < TimeSpan.FromHours(12) ||
                startDate.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("", "فروشنده عزیز تاریخ شروع تخفیف نباید کواه تر از 12 ساعت باشد.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }



            var shopperProductDiscount = await _discountService.GetLastShopperProductColorDiscountAsync(model.ShopperProductColorId);

            if (shopperProductDiscount == null)
            {
                ModelState.AddModelError("", "فروشنده عزیز تاریخ شروع تخفیف نباید کواه تر از 12 ساعت باشد.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            shopperProductDiscount.StartDate = startDate;
            shopperProductDiscount.EndDate = endDate;
            shopperProductDiscount.DiscountPercent = model.DiscountPercent;

            var result = await _discountService.EditShopperProductDiscountAsync(shopperProductDiscount);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "" });
            }

            ModelState.AddModelError("", "متاسفانه خطایی هنگام ثبت تخفیف رخ داده است. لطفا دوباره تلاش کنید.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }


        #endregion

        #region show request

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> ShowShopperProductRequest(string shopperProductRequestId)
        {
            if (string.IsNullOrEmpty(shopperProductRequestId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var model = await _shopperService.GetShopperProductRequestForShowShopperAsync(shopperProductRequestId);

            if (model == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            return View(model);
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> ShowShopperProductColorRequest(string shopperProductColorRequestId)
        {
            if (string.IsNullOrEmpty(shopperProductColorRequestId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var model = await _shopperService.GetShopperProductColorRequestForShowShopperAsync(shopperProductColorRequestId);

            if (model == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            return View(model);
        }


        #endregion

        // this method is for changing page or filter of shoppers list
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> ShopperRequestsList(string filter, string type, int pageId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            string shopperId = await _shopperService.GetShopperIdOrUserAsync(userId);

            return ViewComponent("ShopperRequestsForShowShopperComponent", new { shopperId, type, pageId, filter });
        }


        [HttpPost]
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