using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Generator;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Permission;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    public class UserManagerController : Controller
    {
        #region constructor

        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IShopperService _shopperService;
        private readonly IOriginService _originService;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public UserManagerController(IUserService userService, IPermissionService permissionService, IShopperService shopperService, IOriginService originService, IProductService productService, ICartService cartService)
        {
            _userService = userService;
            _permissionService = permissionService;
            _shopperService = shopperService;
            _originService = originService;
            _productService = productService;
            _cartService = cartService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View(_userService.GetUsersInformation());
        }

        [HttpGet]
        [NoDirectAccess]
        public IActionResult UserRolesList(string userId, string filter, int pageId)
        {
            return ViewComponent("UserRolesComponent", new { userId, pageId, filter });
        }

        #region add or edit

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditUser(string userId = "")
        {
            if (userId == "")
            {
                var model = new AddOrEditUserForAdminViewModel()
                {
                    Roles = await _permissionService.GetRolesAsync()
                };

                return View(model);
            }
            else
            {
                if (!await _userService.IsUserExistAsync(userId))
                    return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

                var model = await _userService.GetUserDataForAdminAsync(userId);

                if (model == null)
                    return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });


                return View(model);
            }
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditUser(AddOrEditUserForAdminViewModel model)
        {
            model.Roles = await _permissionService.GetRolesAsync();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            if (string.IsNullOrEmpty(model.UserId))
            {
                var user = new User
                {
                    FullName = model.FullName,
                    RegisterDate = DateTime.Now,
                    UserAvatar = "userAvatar.jpg",
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    InviteCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                    InviteCount = 0,
                    Score = 0,
                    NationalCode = model.NationalCode,
                    IsBlocked = model.IsBlocked,
                    AccountBalance = model.AccountBalance,
                };

                var addUser = await _userService.AddUserAsync(user);

                if (addUser == ResultTypes.Successful)
                {
                    if (model.SelectedRoles != null && model.SelectedRoles.Any())
                    {
                        await _permissionService.AddUserToRoleAsync(user.UserId, model.SelectedRoles as List<string>);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "عنگام ثبت کاربر مشکلی غیر منتظره به وجود آمده است.");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                }
            }
            else
            {
                var user = await _userService.GetUserByIdAsync(model.UserId);

                if (user == null)
                {
                    ModelState.AddModelError("", "هنگام ویراش کاربر مشکلی غیر منتظره پیش آمده است! لطفا دوباره تلاش کنید.");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                }


                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.InviteCount = 0;
                user.Score = 0;
                user.NationalCode = model.NationalCode;
                user.IsBlocked = model.IsBlocked;
                user.AccountBalance = model.AccountBalance;


                var editUser = await _userService.EditUserAsync(user);

                if (editUser == ResultTypes.Successful)
                {
                    // remove user roles
                    var removeUserRoles = await _permissionService.DeleteAllUserRolesByUserIdAsync(user.UserId);

                    if (removeUserRoles != ResultTypes.Successful)
                    {
                        ModelState.AddModelError("", "هنگام ویراش مقام کاربر مشکلی غیر منتظره پیش آمده است! لطفا دوباره تلاش کنید.");
                        return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                    }

                    if (model.SelectedRoles != null && model.SelectedRoles.Any())
                    {
                        await _permissionService.AddUserToRoleAsync(user.UserId, model.SelectedRoles as List<string>);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "هنگام ویراش کاربر مشکلی غیر منتظره پیش آمده است! لطفا دوباره تلاش کنید.");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                }
            }

            return Json(new { isValid = true, returnUrl = "current" });
        }

        #endregion

        #region remove

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> RemoveUser(string userId)
        {
            if (!await _userService.IsUserExistAsync(userId)) return NotFound();

            await _userService.RemoveUserAsync(userId);

            return RedirectToAction("Index");
        }

        #endregion

        #region detail

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> UserDetail(string userId)
        {
            if (userId == null)
                return BadRequest();

            var user = await _userService.GetUserDetailAsync(userId);
            if (user == null)
                return NotFound();

            return View(user);
        }

        #endregion

        #region add user to shopper

        [HttpGet]
        public async Task<IActionResult> AddToShopper(string userId)
        {
            var userData = await _userService.GetUserDataForEditAsync(userId);

            if (userData == null)
                return NotFound();



            var model = new AddShopperViewModel()
            {
                FullName = userData.FullName,
                Email = userData.Email,
                NationalCode = userData.NationalCode,
                PhoneNumber = userData.PhoneNumber,
                UserId = userData.UserId,
                StoreTitles = _shopperService.GetStoreTitles(),
                States = _originService.GetStates()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToShopper(AddShopperViewModel model)
        {
            model.States = _originService.GetStates();
            model.StoreTitles = _shopperService.GetStoreTitles();

            if (!ModelState.IsValid)
                return View(model);


            //// check images is not script

            if (!model.OnNationalCardImageName.IsImage() ||
                !model.BusinessLicenseImageName.IsImage())
            {
                ModelState.AddModelError("", "فروشنده عزیز لطفا تصاویر خود را درست وارد کنید.");
                return View(model);
            }


            var user = await _userService.GetUserByIdAsync(model.UserId);

            if (user == null)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت نام فروشنده به مشکلی غیر منتظره برخوردیم.");
                return View(model);
            }

            // edit user data
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.NationalCode = model.NationalCode;


            if (await _shopperService.IsUserShopperAsync(user.UserId))
            {
                string shopperId = await _shopperService.GetShopperIdOrUserAsync(user.UserId);

                var deleteShopper = await _shopperService.DeleteShopperAsync(shopperId);

                if (deleteShopper != ResultTypes.Successful)
                {
                    ModelState.AddModelError("", "متاسفانه هنگام ثبت نام فروشنده به مشکلی غیر منتظره برخوردیم.");
                    return View(model);
                }
            }

            // new shopper
            var shopper = new Shopper()
            {
                StoreName = model.StoreName,
                BirthDay = model.BirthDay.ConvertPersianDateToEnglishDate(),
                RegisterShopper = DateTime.Now,
                IsActive = true,
                User = user,
                UserId = user.UserId
            };

            // shopper images
            if (model.OnNationalCardImageName.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "shoppersCardImages");
                string imageName = await ImageConvertor.CreateNewImage(model.OnNationalCardImageName, path);
                shopper.OnNationalCardImageName = imageName;
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

            var addShopperRes = await _shopperService.AddShopperAsync(shopper);

            if (addShopperRes != ResultTypes.Successful)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت نام فروشنده به مشکلی غیر منتظره برخوردیم.");
                return View(model);
            }

            // shopper store titles
            var addStoreTitle = await _shopperService.AddShopperStoreTitleAsync(shopper.ShopperId, model.SelectedStoreTitles as List<int>);

            if (addStoreTitle != ResultTypes.Successful)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت نام فروشنده به مشکلی غیر منتظره برخوردیم.");
                return View(model);
            }

            // add tore address
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
                ModelState.AddModelError("", "متاسفانه هنگام ثبت نام فروشنده به مشکلی غیر منتظره برخوردیم.");
                return View(model);
            }

            // add user role
            var addRole = await _permissionService.AddUserToRoleAsync(user.UserId, "Shopper");

            if (addRole != ResultTypes.Successful)
            {
                ModelState.AddModelError("", "متاسفانه هنگام ثبت نام فروشنده به مشکلی غیر منتظره برخوردیم.");
                return View(model);
            }

            return RedirectToAction("UserDetail", new { userId = user.UserId });
        }

        #endregion


        #region new address

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> NewAddress(string userId)
        {
            if (!await _userService.IsUserExistAsync(userId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            ViewBag.States = _originService.GetStates();

            ViewData["selectedState"] = 0;

            return View(new Address()
            {
                UserId = userId
            });
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> NewAddress(Address model, int selectedState)
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


            var address = new Address()
            {
                UserId = model.UserId,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                CityId = model.CityId,
                Plaque = model.Plaque,
                PostalCode = model.PostalCode,
                AddressText = model.AddressText
            };

            var result = await _userService.AddUserAddressAsync(address);

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

        #region edit address

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditAddress(string addressId)
        {
            if (string.IsNullOrEmpty(addressId))
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var address = await _userService.GetAddressByIdAsync(addressId);

            if (address is null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            var stateId = await _originService.GetStateIdOfCityAsync(address.CityId);

            if (stateId == 0)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            ViewData["selectedState"] = stateId;
            ViewBag.States = _originService.GetStates();
            ViewBag.Cities = _originService.GetCitiesOfState(stateId);

            return View(address);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> EditAddress(Address model, int selectedState)
        {
            ViewBag.States = _originService.GetStates();
            ViewBag.Cities = _originService.GetCitiesOfState(selectedState);

            ViewData["selectedState"] = selectedState;
            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            var address = await _userService.GetAddressByIdAsync(model.AddressId);

            if (address is null)
            {
                ModelState.AddModelError("", "هنگام ویرایش ادرس به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            address.FullName = model.FullName;
            address.CityId = model.CityId;
            address.Plaque = model.Plaque;
            address.PhoneNumber = model.PhoneNumber;
            address.PostalCode = model.PostalCode;
            address.AddressText = model.AddressText;

            var result = await _userService.EditUserAddressAsync(address);

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

        #region Delete address

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> DeleteAddress(string addressId)
        {
            var deleteAddress = await _userService.DeleteUserAddressAsync(addressId);

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

        [HttpGet]
        [NoDirectAccess]
        public IActionResult ListFavoriteProducts(string userId, int pageId, string sortBy)
        {
            return ViewComponent("UserFavoriteProductsComponent", new { userId, pageId, sortBy });
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> DeleteFavoriteProduct(string userId, string shopperProductColorId)
        {
            var res = await _productService.DeleteFavoriteProductAsync(userId, shopperProductColorId);

            if (res == ResultTypes.Successful)
            {
                return Json(new { isValid = true, errorType = "success", errorText = "کالا با موفقیت از علاقه مندی ها حذف شد.", returnUrl = "current" });
            }
            else
            {
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });
            }
        }

        #region order

        [HttpGet]
        [NoDirectAccess]
        public IActionResult UserOrdersList(string userId, string type, int pageId)
        {
            return ViewComponent("UserOrdersComponent", new { userId, type, pageId, orderBy = "news" });
        }

        [HttpGet]
        [Route("[action]/{userId}/{trackingCode}")]
        public async Task<IActionResult> UserOrderDetail(string userId, string trackingCode)
        {
            if (string.IsNullOrEmpty(trackingCode))
                return BadRequest();

            var orderId = await _cartService.GetOrderIdByTrackingCodeAsync(trackingCode);

            if (string.IsNullOrEmpty(orderId))
                return NotFound();

            if (!await _cartService.IsUserOrderAsync(userId, orderId))
                return NotFound();

            var order = await _cartService.GetFullOrderForShowAsync(orderId);

            if (order == null)
                return NotFound();

            return View(order);
        }

        #endregion
    }
}
