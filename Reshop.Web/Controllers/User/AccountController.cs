using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Generator;
using Reshop.Application.Interfaces.Product;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Validations.Google;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Reshop.Application.Interfaces.Conversation;
using Reshop.Application.Security.GoogleRecaptcha;

namespace Reshop.Web.Controllers.User
{
    public class AccountController : Controller
    {
        #region constructor

        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IOriginService _originService;
        private readonly ICommentService _commentService;
        private readonly IOptions<GoogleReCaptchaKey> _captchaKey;
        private readonly IDataProtector _dataProtector;

        public AccountController(IUserService userService, ICartService cartService, IProductService productService, IOriginService stateService, IOptions<GoogleReCaptchaKey> captchaKey, IDataProtectionProvider dataProtectionProvider, ICommentService commentService)
        {
            _userService = userService;
            _cartService = cartService;
            _productService = productService;
            _originService = stateService;
            _captchaKey = captchaKey;
            _commentService = commentService;
            _dataProtector = dataProtectionProvider.CreateProtector("Reshop.Web.Controllers.User.AccountController",
                new string[] { "AccountController" });
        }


        #endregion

        #region register

        [Route("Register")]
        [HttpGet]
        public IActionResult Register(string inviteCode = null)
        {
            ViewData["inviteCode"] = inviteCode;
            return View();
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(IFormCollection form, RegisterViewModel model, string inviteCode = null)
        {
            #region recaptcha

            bool googleRecaptcha = GoogleReCaptchaValidation.IsGoogleRecaptchaValid(form, _captchaKey.Value.RecapchaSecretKey);

            if (!googleRecaptcha)
            {
                ViewBag.RecaptchaErrorMessage = "لطفا هویت خود را تایید کنید.";
                return View();
            }

            #endregion



            ViewData["inviteCode"] = inviteCode;
            if (!ModelState.IsValid) return View(model);


            var user = new Domain.Entities.User.User
            {
                FullName = model.FullName,
                RegisterDate = DateTime.Now,
                UserAvatar = "userAvatar.jpg",
                PhoneNumber = model.PhoneNumber,
                InviteCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                InviteCount = 0,
                Score = 0,
                NationalCode = "-",
                Email = "-",
                IsBlocked = false,
            };

            var result = await _userService.AddUserAsync(user);

            if (result == ResultTypes.Successful)
            {
                #region send email

                //var emailBody = _viewRender.RenderToStringAsync("_EmailActivation", user);

                //await _messageSender.SendEmailAsync(user.Email, "تایید ایمیل", emailBody, true);

                #endregion


                if (inviteCode != null)
                {
                    await _userService.AddUserToInvitesAsync(inviteCode, user.UserId);
                }

                return Redirect("/");
            }

            ModelState.AddModelError("", "کاربر گرامی متاسفانه هنگام ثبت نام با مشگلی غیر منتظره مواجه شده ایم. لطفا با پشتیبانی تماس بگیرید.");
            return View(model);
        }

        #endregion

        #region login

        [Route("Login")]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }



        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(IFormCollection form, LoginViewModel model, string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/");

            #region recaptcha

            bool googleRecaptcha = GoogleReCaptchaValidation.IsGoogleRecaptchaValid(form, _captchaKey.Value.RecapchaSecretKey);

            if (!googleRecaptcha)
            {
                ViewBag.RecaptchaErrorMessage = "لطفا هویت خود را تایید کنید.";
                return View();
            }

            #endregion

            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "تلاش شما برای ورود موفقیت امیز نبود. شما میتوانید بار دیگر اقدام به ورود کنید.");
                return View(model);
            }




            var user = await _userService.GetUserByPhoneNumberAsync(model.PhoneNumber);

            if (user != null)
            {
                // settings of user data for login
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Actor,user.UserAvatar.ToString()),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                };

                // login user in site
                await HttpContext.SignInAsync(principal, properties);


                if (returnUrl != null && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return Redirect("/");
            }

            ModelState.AddModelError("", "تلاش شما برای ورود موفقیت امیز نبود. شما میتوانید بار دیگر اقدام به ورود کنید.");
            return View(model);
        }

        #endregion

        #region logout

        [Route("Logout")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        #endregion

        #region dashboard

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userService.GetUserByIdAsync(userId);

            if (user is null)
                return NotFound();

            return View(user);
        }

        #endregion

        #region edit user data

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
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });


            try
            {
                model.UserId = _dataProtector.Unprotect(model.UserId);
            }
            catch
            {
                ModelState.AddModelError("", "هنگام ویرایش اطلاعات به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            var user = await _userService.GetUserByIdAsync(model.UserId);

            if (user is null)
            {
                ModelState.AddModelError("", "هنگام ویرایش اطلاعات به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.NationalCode = model.NationalCode;
            user.Email = model.Email;

            var result = await _userService.EditUserAsync(user);

            if (result == ResultTypes.Successful)
            {
                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                ModelState.AddModelError("", "هنگام ویرایش اطلاعات به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion

        #region address

        [HttpGet]
        public IActionResult Address()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var addresses = _userService.GetUserAddresses(userId);

            return View(addresses);
        }

        #endregion

        #region new address

        [HttpGet]
        [NoDirectAccess]
        public IActionResult NewAddress()
        {
            ViewBag.States = _originService.GetStates();

            ViewData["selectedState"] = 0;

            return View(new Address());
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

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
                UserId = userId,
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

            address.UserId = _dataProtector.Protect(address.UserId);

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


            try
            {
                model.UserId = _dataProtector.Unprotect(model.UserId);
            }
            catch
            {
                ModelState.AddModelError("", "هنگام ویرایش ادرس به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            var address = await _userService.GetAddressByIdAsync(model.AddressId);

            if (address is null)
            {
                ModelState.AddModelError("", "هنگام ویرایش ادرس به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (address.UserId != userId)
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
                return Json(new { isValid = true });
            }
            else
            {
                ModelState.AddModelError("", "هنگام ویرایش ادرس به مشکلی غیر منتظره برخوردیم. لطفا با پشتیبانی تماس بگیرید.");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        #endregion

        #region order

        [HttpGet]
        public IActionResult UnFinallyOrders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_cartService.GetNotReceivedOrders(userId));
        }

        [HttpGet]
        public IActionResult FinallyOrders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_cartService.GetReceivedOrders(userId));
        }

        [Route("[action]/{trackingCode}")]
        public async Task<IActionResult> OrderDetail(string trackingCode)
        {
            if (string.IsNullOrEmpty(trackingCode))
                return BadRequest();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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

        #region question and comment

        [HttpGet]
        public IActionResult Questions()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_userService.GetUserQuestionsForShow(userId));
        }

        [HttpGet]
        public IActionResult Comments()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View(_commentService.GetUserCommentsForShow(userId));
        }

        #endregion

        #region favorite products

        [HttpGet]
        public async Task<IActionResult> FavoriteProducts(string type = "all", string sortBy = "news", int pageId = 1)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.SortBy = sortBy.ToLower();
            ViewBag.Type = type.ToLower();

            return View(await _productService.GetUserFavoriteProductsWithPagination(userId, type, sortBy, pageId, 18));
        }

        #endregion
    }
}
