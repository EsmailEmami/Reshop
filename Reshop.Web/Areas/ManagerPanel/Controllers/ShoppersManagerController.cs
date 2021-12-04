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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    public class ShoppersManager : Controller
    {
        private readonly IShopperService _shopperService;
        private readonly IProductService _productService;
        private readonly IDataProtector _dataProtector;
        private readonly IBrandService _brandService;
        private readonly IDiscountService _discountService;
        private readonly IOriginService _originService;
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;

        public ShoppersManager(IShopperService shopperService, IProductService productService, IDataProtectionProvider dataProtectionProvider, IBrandService brandService, IDiscountService discountService, IOriginService originService, IPermissionService permissionService, IUserService userService)
        {
            _shopperService = shopperService;
            _productService = productService;
            _brandService = brandService;
            _discountService = discountService;
            _originService = originService;
            _permissionService = permissionService;
            _userService = userService;
            _dataProtector = dataProtectionProvider.CreateProtector("Reshop.Web.Areas.ManagerPanel.Controllers.ShoppersManager",
                new string[] { "ShoppersManager" });
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _shopperService.GetShoppersGeneralDataForAdminAsync();

            return View(data);
        }

        #region shopper

        // this method is for changing page or filter of shoppers list
        [HttpGet]
        [NoDirectAccess]
        public IActionResult ShoppersList(string type, int pageId, string filter)
        {
            return ViewComponent("ShoppersListComponent", new { type, pageId, filter });
        }

        [HttpGet("[action]/{shopperId}")]
        public async Task<IActionResult> ShopperDetail(string shopperId)
        {
            if (shopperId == null) return NotFound();

            var model = await _shopperService.GetShopperDataForAdminAsync(shopperId);

            if (model == null) return NotFound();

            return View(model);
        }

        // this method is for changing page or filter of shoppers list
        [HttpGet]
        [NoDirectAccess]
        public IActionResult ShopperRequestsList(string shopperId, string filter, string type, int pageId)
        {
            return ViewComponent("ShopperRequestsComponent", new { shopperId, type, pageId, filter });
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> ShowShopperProductRequest(string shopperProductRequestId)
        {
            if (string.IsNullOrEmpty(shopperProductRequestId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var model = await _shopperService.GetShopperProductRequestForShowAsync(shopperProductRequestId);

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

            var model = await _shopperService.GetShopperProductColorRequestForShowAsync(shopperProductColorRequestId);

            if (model == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> FinishShopperProductRequest(FinishShopperRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "ShopperManager/_FinishShopperProductRequest", model) });

            var res = await _shopperService.FinishShopperProductRequestAsync(model);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه مشکلی پیش آمده است. لطفا دوباره تلاش کنید");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "ShopperManager/_FinishShopperProductRequest", model) });
            }
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> FinishShopperProductColorRequest(FinishShopperRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "ShopperManager/_FinishShopperProductRequest", model) });

            var res = await _shopperService.FinishShopperProductColorRequestAsync(model);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                ModelState.AddModelError("", "متاسفانه مشکلی پیش آمده است. لطفا دوباره تلاش کنید");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, "ShopperManager/_FinishShopperProductRequest", model) });
            }
        }

        #endregion

        #region edit shopper

        [HttpGet]
        public async Task<IActionResult> EditShopper(string shopperId)
        {
            var shopperData = await _shopperService.GetShopperDataForEditAsync(shopperId);

            if (shopperData == null)
                return NotFound();


            return View(shopperData);
        }

        [HttpPost]
        public async Task<IActionResult> EditShopper(EditShopperViewModel model)
        {
            model.StoreTitles = _shopperService.GetStoreTitles();
            model.Roles = await _permissionService.GetRolesAsync();

            if (!ModelState.IsValid)
                return View(model);

            // check user birthday
            DateTime birthDay = model.BirthDay.ConvertPersianDateToEnglishDate();

            if (birthDay.Year >= DateTime.Now.AddYears(-18).Year)
            {
                ModelState.AddModelError("BirthDay", "لطفا سال تولد خود را به درستی وارد کنید.");
                return View(model);
            }

            // check images is not script
            if (model.OnNationalCardImage != null && !model.OnNationalCardImage.IsImage() ||
                model.BusinessLicenseImage != null && !model.BusinessLicenseImage.IsImage())
            {
                ModelState.AddModelError("", "فروشنده عزیز لطفا تصاویر خود را درست وارد کنید.");
                return View(model);
            }

            var shopper = await _shopperService.GetShopperByIdAsync(model.ShopperId);

            if (shopper == null)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش فروشنده به مشکلی غیر منتظره برخوردیم.");
                return View(model);
            }

            var user = await _userService.GetUserByIdAsync(shopper.UserId);

            if (user == null)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش فروشنده به مشکلی غیر منتظره برخوردیم.");
                return View(model);
            }

            // edit user data
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.NationalCode = model.NationalCode;

            // edit shopper data
            shopper.StoreName = model.StoreName;
            shopper.BirthDay = birthDay;
            shopper.IsActive = model.IsActive;


            // shopper images
            if (shopper.OnNationalCardImageName == model.OnNationalCardImageName && model.OnNationalCardImage != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "shoppersCardImages");


                // delete image
                ImageConvertor.DeleteImage($"{path}/{shopper.OnNationalCardImageName}");

                // add new image
                string imageName = await ImageConvertor.CreateNewImage(model.OnNationalCardImage, path);
                shopper.OnNationalCardImageName = imageName;
            }

            if (shopper.BusinessLicenseImageName == model.BusinessLicenseImageName && model.BusinessLicenseImage != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "shoppersCardImages");


                // delete image
                ImageConvertor.DeleteImage($"{path}/{shopper.BusinessLicenseImageName}");

                // add new image
                string imageName = await ImageConvertor.CreateNewImage(model.BusinessLicenseImage, path);
                shopper.BusinessLicenseImageName = imageName;
            }

            var editUser = await _userService.EditUserAsync(user);

            if (editUser != ResultTypes.Successful)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش فروشنده به مشکلی غیر منتظره برخوردیم.");
                return View(model);
            }

            var editShopper = await _shopperService.EditShopperAsync(shopper);

            if (editShopper != ResultTypes.Successful)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش فروشنده به مشکلی غیر منتظره برخوردیم.");
                return View(model);
            }

            // delete shopper store titles
            await _shopperService.DeleteShopperStoreTitlesAsync(shopper.ShopperId);

            // add new shopper store titles
            var addStoreTitle = await _shopperService.AddShopperStoreTitleAsync(shopper.ShopperId, model.SelectedStoreTitles as List<int>);

            if (addStoreTitle != ResultTypes.Successful)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش فروشنده به مشکلی غیر منتظره برخوردیم.");
                return View(model);
            }

            // delete user roles
            await _permissionService.DeleteAllUserRolesByUserIdAsync(user.UserId);

            // add user role
            if (model.SelectedRoles != null && model.SelectedRoles.Any())
            {
                var addRole = await _permissionService.AddUserToRoleAsync(user.UserId, model.SelectedRoles as List<string>);

                if (addRole != ResultTypes.Successful)
                {
                    ModelState.AddModelError("", "متاسفانه هنگام ویرایش فروشنده به مشکلی غیر منتظره برخوردیم.");
                    return View(model);
                }
            }

            return RedirectToAction("ShopperDetail", new { shopperId = shopper.ShopperId });
        }

        #endregion

        #region shopper product

        // this method is for changing page or filter of shopper Products list
        [HttpGet]
        [NoDirectAccess]
        public IActionResult ShopperProductsList(string shopperId, string type = "all", int pageId = 1, string filter = "")
        {
            return ViewComponent("ShopperProductsOfShopperComponent", new { shopperId, type, pageId, filter });
        }

        [HttpGet]
        public async Task<IActionResult> ShopperProductDetail(string shopperProductId)
        {
            if (shopperProductId == null)
                return NotFound();

            var model = await _productService.GetProductDetailForShopperAsync(shopperProductId);

            if (model == null)
                return NotFound();

            ViewBag.ShopperProductId = shopperProductId;

            return View(model);
        }

        #endregion

        #region add or edit shopper product

        // in this method we create new shopper product for shopper 
        // we do not need validation product in this method
        // this method open in modal
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddShopperProduct(string shopperId)
        {
            if (shopperId == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            if (!await _shopperService.IsShopperExistAsync(shopperId))
                return Json(new { isValid = false, errorType = "warning", errorText = "فروشنده یافت نشد! لطفا دوباره تلاش کنید." });

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
                ModelState.AddModelError("", "این کالا قبلا توسط این فروشنده ثبت شده است. ");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            if (await _shopperService.IsAnyActiveShopperProductRequestExistAsync(model.ShopperId, model.ProductId, true))
            {
                ModelState.AddModelError("", "فروشنده یک درخواست در حال بررسی دارد.");
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
                IsSuccess = true,
                IsRead = true,
            };

            var result = await _shopperService.AddShopperProductRequestAsync(shopperProductRequest);

            if (result == ResultTypes.Successful)
            {
                var shopperProduct = new ShopperProduct()
                {
                    CreateDate = DateTime.Now,
                    IsActive = model.IsActive,
                    IsFinally = true,
                    ProductId = model.ProductId,
                    ShopperId = model.ShopperId,
                    Warranty = model.Warranty
                };

                var addShopperProduct = await _shopperService.AddShopperProductAsync(shopperProduct);

                if (addShopperProduct == ResultTypes.Successful)
                {
                    return Json(new { isValid = true, returnUrl = "current" });
                }
            }

            ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        // we do not need validation edit product in this method
        // this method open in modal
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditShopperProduct(string shopperProductId)
        {
            if (shopperProductId == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            if (!await _productService.IsShopperProductExistAsync(shopperProductId))
                return Json(new { isValid = false, errorType = "warning", errorText = "فروشنده یافت نشد! لطفا دوباره تلاش کنید." });




            var model = await _shopperService.GetShopperProductDataForEditAsync(shopperProductId);

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
                ModelState.AddModelError("", "فروشنده یک درخواست در حال بررسی دارد.");
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
                IsSuccess = true,
                Reason = ShopperProductRequestReasons.AdminEditedProduct(),
                IsRead = true,
            };

            var result = await _shopperService.AddShopperProductRequestAsync(shopperProductRequest);

            if (result == ResultTypes.Successful)
            {
                var shopperProduct = await _productService.GetShopperProductAsync(model.ProductId, model.ShopperId);

                if (shopperProduct != null)
                {
                    shopperProduct.IsActive = model.IsActive;
                    shopperProduct.Warranty = model.Warranty;


                    var editShopperProduct = await _shopperService.EditShopperProductAsync(shopperProduct);

                    if (editShopperProduct == ResultTypes.Successful)
                    {
                        return Json(new { isValid = true, returnUrl = "current" });
                    }
                }

                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        #endregion

        #region shopper product color

        // add color request
        [HttpGet]
        [NoDirectAccess]
        public IActionResult AddColorToShopperProduct(string shopperProductId)
        {
            if (shopperProductId == null)
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
        public async Task<IActionResult> AddColorToShopperProduct(AddColorToShopperProductViewModel model)
        {
            ViewBag.Colors = _shopperService.GetColorsIdAndName();
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            if (await _shopperService.IsShopperProductColorExistAsync(model.ShopperProductId, model.ColorId))
            {
                ModelState.AddModelError("", "این رنگ قبلا توسط فروشنده ثبت شده است.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            }


            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shopperProductColorRequest = new ShopperProductColorRequest()
            {
                ShopperProductId = model.ShopperProductId,
                RequestType = true,
                RequestDate = DateTime.Now,
                Price = model.Price,
                QuantityInStock = model.QuantityInStock,
                IsSuccess = true,
                IsRead = true,
                IsActive = model.IsActive,
                RequestUserId = userId,
                ColorId = model.ColorId
            };

            var result = await _shopperService.AddShopperProductColorRequestAsync(shopperProductColorRequest);

            if (result == ResultTypes.Successful)
            {
                var shopperProductColor = new ShopperProductColor()
                {
                    ColorId = model.ColorId,
                    CreateDate = DateTime.Now,
                    IsActive = model.IsActive,
                    IsFinally = true,
                    Price = model.Price,
                    QuantityInStock = model.QuantityInStock,
                    ShortKey = NameGenerator.GenerateShortKey(),
                    ShopperProductId = model.ShopperProductId
                };

                var addShopperProductColor = await _shopperService.AddShopperProductColorAsync(shopperProductColor);

                if (addShopperProductColor == ResultTypes.Successful)
                {
                    return Json(new { isValid = true, returnUrl = "current" });
                }

                ModelState.AddModelError("", "متاسفانه هنگام افزودن رنگ به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            ModelState.AddModelError("", "متاسفانه هنگام افزودن رنگ به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        // edit color information
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditColorOfShopperProduct(string shopperProductId, int colorId)
        {
            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperProductId, colorId);

            var model = await _productService.GetShopperProductColorForEditAsync(shopperProductColorId);

            if (model == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            model.ShopperProductColorId = _dataProtector.Protect(model.ShopperProductColorId);

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> EditColorOfShopperProduct(EditColorOfShopperProductViewModel model)
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


            var shopperProductColor = await _shopperService.GetShopperProductColorAsync(model.ShopperProductColorId);
            if (shopperProductColor is null)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            var shopperProductColorRequest = new ShopperProductColorRequest()
            {
                ShopperProductId = shopperProductColor.ShopperProductId,
                RequestType = false,
                RequestDate = DateTime.Now,
                Price = model.Price,
                QuantityInStock = model.QuantityInStock,
                IsSuccess = true,
                IsRead = true,
                IsActive = model.IsActive,
                RequestUserId = userId,
                ColorId = shopperProductColor.ColorId
            };

            var result = await _shopperService.AddShopperProductColorRequestAsync(shopperProductColorRequest);

            if (result == ResultTypes.Successful)
            {
                shopperProductColor.IsActive = model.IsActive;
                shopperProductColor.Price = model.Price;
                shopperProductColor.QuantityInStock = model.QuantityInStock;

                var edit = await _shopperService.EditShopperProductColorAsync(shopperProductColor);

                if (edit == ResultTypes.Successful)
                {
                    return Json(new { isValid = true, returnUrl = "current" });
                }
            }

            ModelState.AddModelError("", "متاسفانه هنگام ویرایش محصول به مشکلی غیر منتظره برخوردیم.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> ShopperProductColorDetail(string shopperProductId, int colorId)
        {
            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperProductId, colorId);


            if (shopperProductColorId is null)
                return Json(new { isValid = false, errorType = "warning", errorText = "متاسفانه درخواست شما به مشکل مواجه شد. لطفا دوباره تلاش کنید." });

            var model = await _shopperService.GetShopperProductColorDetailAsync(shopperProductColorId);

            if (model is null)
                return Json(new { isValid = false, errorType = "warning", errorText = "متاسفانه درخواست شما به مشکل مواجه شد. لطفا دوباره تلاش کنید." });

            ViewBag.ShopperProductId = shopperProductId;

            return View(model);
        }

        #endregion

        #region discount

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> ShopperProductDiscountDetail(string shopperProductId, int colorId)
        {
            if (shopperProductId == null || colorId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperProductId, colorId);

            if (shopperProductColorId is null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            var model = await _discountService.GetShopperProductColorDiscountsGeneralDataAsync(shopperProductColorId);

            if (model is null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            ViewBag.ShopperProductId = shopperProductId;

            return View(model);
        }

        // new discount
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddShopperProductColorDiscount(string shopperProductId, int colorId)
        {
            if (shopperProductId == null && colorId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperProductId, colorId);

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
        public async Task<IActionResult> AddShopperProductColorDiscount(AddOrEditShopperProductDiscountViewModel model)
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

            if (await _discountService.IsActiveShopperProductColorDiscountExistsAsync(model.ShopperProductColorId))
            {
                ModelState.AddModelError("", "فروشنده عزیز شما یک تخفیف فعال دارید.");
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
                return Json(new { isValid = true, returnUrl = "current" });
            }

            ModelState.AddModelError("", "متاسفانه خطایی هنگام ثبت تخفیف رخ داده است. لطفا دوباره تلاش کنید.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }

        // edit discount
        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditShopperProductColorDiscount(string shopperProductId, int colorId)
        {
            if (shopperProductId == null && colorId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });

            var shopperProductColorId = await _shopperService.GetShopperProductColorIdAsync(shopperProductId, colorId);

            if (string.IsNullOrEmpty(shopperProductColorId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });


            if (!await _discountService.IsActiveShopperProductColorDiscountExistsAsync(shopperProductColorId))
                return Json(new { isValid = false, errorType = "warning", errorText = "فروشنده عزیز شما یک تخفیف فعال دارید." });

            var discount = await _discountService.GetLastShopperProductColorDiscountAsync(shopperProductColorId);


            var model = new AddOrEditShopperProductDiscountViewModel()
            {
                ShopperProductColorId = _dataProtector.Protect(shopperProductColorId),
                StartDate = discount.StartDate.ToString(),
                EndDate = discount.EndDate.ToString(),
                DiscountPercent = discount.DiscountPercent,
            };

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> EditShopperProductColorDiscount(AddOrEditShopperProductDiscountViewModel model)
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

            DateTime startDate = model.StartDate.ConvertPersianDateTimeToEnglishDateTime();
            DateTime endDate = model.EndDate.ConvertPersianDateTimeToEnglishDateTime();



            if (startDate >= endDate || endDate.Subtract(startDate) < TimeSpan.FromHours(12) ||
                startDate.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("", "فروشنده عزیز تاریخ شروع تخفیف نباید کواه تر از 12 ساعت باشد.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            var discount = await _discountService.GetLastShopperProductColorDiscountAsync(model.ShopperProductColorId);

            discount.StartDate = startDate;
            discount.EndDate = endDate;
            discount.DiscountPercent = model.DiscountPercent;

            var result = await _discountService.EditShopperProductDiscountAsync(discount);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }

            ModelState.AddModelError("", "متاسفانه خطایی هنگام ثبت تخفیف رخ داده است. لطفا دوباره تلاش کنید.");
            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
        }


        #endregion

        #region storeTitleList

        [HttpGet]
        public IActionResult ManageStoreTitles()
        {
            return View(_shopperService.GetStoreTitles());
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditStoreTitle(int storeTitleId = 0)
        {
            if (storeTitleId == 0)
            {
                return View(new StoreTitle());
            }
            else
            {
                var storeTitle = await _shopperService.GetStoreTitleByIdAsync(storeTitleId);

                if (storeTitle == null)
                    return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است. لطفا دوباره تلاش کنید." });


                return View(storeTitle);
            }
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditStoreTitle(StoreTitle model)
        {
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            if (model.StoreTitleId == 0)
            {
                var storeTitle = new StoreTitle()
                {
                    StoreTitleName = model.StoreTitleName
                };


                var result = await _shopperService.AddStoreTitleAsync(storeTitle);

                if (result == ResultTypes.Successful)
                {
                    return Json(new { isValid = true, returnUrl = "current" });
                }

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام افزودن عنوان به مشکل برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
            else
            {
                var storeTitle = await _shopperService.GetStoreTitleByIdAsync(model.StoreTitleId);

                if (storeTitle == null)
                {
                    ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ویرایش عنوان به مشکل برخوردیم.");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                }

                storeTitle.StoreTitleName = model.StoreTitleName;

                var result = await _shopperService.EditStoreTitleAsync(storeTitle);

                if (result == ResultTypes.Successful)
                {
                    return Json(new { isValid = true, returnUrl = "current" });
                }

                ModelState.AddModelError("", "ادمین عزیز متاسفانه هنگام ویرایش عنوان به مشکل برخوردیم.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStoreTitle(int storeTitleId)
        {
            var res = await _shopperService.DeleteStoreTitleAsync(storeTitleId);

            if (res != ResultTypes.Successful)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(ManageStoreTitles));
        }

        #endregion

        #region Add Store Address

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddStoreAddress(string shopperId)
        {
            if (shopperId == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            if (!await _shopperService.IsShopperExistAsync(shopperId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            ViewBag.States = _originService.GetStates();

            ViewData["selectedState"] = 0;

            return View(new StoreAddress()
            {
                ShopperId = shopperId
            });
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddStoreAddress(StoreAddress model, int selectedState)
        {
            ViewBag.States = _originService.GetStates();
            ViewBag.Cities = _originService.GetCitiesOfState(selectedState);

            ViewData["selectedState"] = selectedState;

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            var city = await _originService.GetCityByIdAsync(model.CityId);


            // error while is problem from city
            if (city == null)
            {
                ModelState.AddModelError("", "هنگام ثبت ادرس به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
            else if (city.StateId != selectedState)
            {
                ModelState.AddModelError("", "هنگام ثبت ادرس به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }


            var storeAddress = new StoreAddress()
            {
                ShopperId = model.ShopperId,
                StoreName = model.StoreName,
                Plaque = model.Plaque,
                CityId = model.CityId,
                PostalCode = model.PostalCode,
                AddressText = model.AddressText,
                LandlinePhoneNumber = model.LandlinePhoneNumber
            };

            var result = await _shopperService.AddStoreAddressAsync(storeAddress);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                ModelState.AddModelError("", "هنگام ثبت ادرس به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion

        #region Edit Store Address

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditStoreAddress(string storeAddressId)
        {
            if (string.IsNullOrEmpty(storeAddressId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var storeAddress = await _shopperService.GetStoreAddressByIdAsync(storeAddressId);

            if (storeAddress is null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var stateId = await _originService.GetStateIdOfCityAsync(storeAddress.CityId);

            if (stateId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            ViewData["selectedState"] = stateId;
            ViewBag.States = _originService.GetStates();
            ViewBag.Cities = _originService.GetCitiesOfState(stateId);

            return View(storeAddress);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> EditStoreAddress(StoreAddress model, int selectedState)
        {
            ViewBag.States = _originService.GetStates();
            ViewBag.Cities = _originService.GetCitiesOfState(selectedState);

            ViewData["selectedState"] = selectedState;
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            var storeAddress = await _shopperService.GetStoreAddressByIdAsync(model.StoreAddressId);

            if (storeAddress is null)
            {
                ModelState.AddModelError("", "هنگام ویرایش ادرس به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            storeAddress.StoreName = model.StoreName;
            storeAddress.CityId = model.CityId;
            storeAddress.Plaque = model.Plaque;
            storeAddress.LandlinePhoneNumber = model.LandlinePhoneNumber;
            storeAddress.PostalCode = model.PostalCode;
            storeAddress.AddressText = model.AddressText;

            var result = await _shopperService.EditStoreAddressAsync(storeAddress);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                ModelState.AddModelError("", "هنگام ویرایش ادرس به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion

        #region Delete Store Address

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> DeleteStoreAddress(string storeAddressId)
        {
            var deleteAddress = await _shopperService.DeleteStoreAddressAsync(storeAddressId);

            if (deleteAddress == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });
            }
        }

        #endregion

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> UnAvailableShopperProduct(string shopperProductId)
        {
            if (string.IsNullOrEmpty(shopperProductId))
            {
                return Json(new { isValid = false });
            }


            var shopperProduct = await _productService.GetShopperProductAsync(shopperProductId);

            if (shopperProduct is null)
            {
                return Json(new { isValid = false });
            }

            shopperProduct.IsFinally = false;

            var result = await _shopperService.EditShopperProductAsync(shopperProduct);

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
