using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Generator;
using Reshop.Application.Interfaces.Shopper;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security.GoogleRecaptcha;
using Reshop.Application.Senders;
using Reshop.Application.Validations.Google;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Shopper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Reshop.Domain.Entities.User;

namespace Reshop.Web.Controllers.User
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        #region constructor

        private readonly IUserService _userService;
        private readonly IViewRenderService _viewRender;
        private readonly IMessageSender _messageSender;
        private readonly IOptions<GoogleReCaptchaKey> _captchaKey;
        private readonly IShopperService _shopperService;
        private readonly IRoleService _roleService;
        private readonly IStateService _stateService;

        public AccountController(IUserService userService, IViewRenderService viewRender, IMessageSender messageSender, IOptions<GoogleReCaptchaKey> captchaKey, IShopperService shopperService, IRoleService roleService, IStateService stateService)
        {
            _userService = userService;
            _viewRender = viewRender;
            _messageSender = messageSender;
            _captchaKey = captchaKey;
            _shopperService = shopperService;
            _roleService = roleService;
            _stateService = stateService;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string inviteCode = null)
        {
            //#region recaptcha

            //bool googleRecaptcha = GoogleReCaptchaValidation.IsGoogleRecaptchaValid(form, _captchaKey.Value.RecapchaSecretKey);

            //if (!googleRecaptcha)
            //{
            //    ViewBag.RecaptchaErrorMessage = "لطفا هویت خود را تایید کنید.";
            //    return View();
            //}

            //#endregion



            ViewData["inviteCode"] = inviteCode;
            if (!ModelState.IsValid) return View(model);


            var user = new Domain.Entities.User.User
            {
                ActiveCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                FullName = model.FullName,
                RegisteredDate = DateTime.Now,
                UserAvatar = "userAvatar.jpg",
                PhoneNumber = model.PhoneNumber,
                InviteCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                InviteCount = 0,
                Score = 0,
                NationalCode = "-",
                PostalCode = "-",
                IsPhoneNumberActive = false,
                Email = "-",
                IsBlocked = false,
                IsUserShopper = false,
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(/*IFormCollection form,*/ LoginViewModel model, string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/");

            //#region recaptcha

            //bool googleRecaptcha = GoogleReCaptchaValidation.IsGoogleRecaptchaValid(form, _captchaKey.Value.RecapchaSecretKey);

            //if (!googleRecaptcha)
            //{
            //    ViewBag.RecaptchaErrorMessage = "لطفا هویت خود را تایید کنید.";
            //    return View();
            //}

            //#endregion

            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "تلاش شما برای ورود موفقیت امیز نبود. شما میتوانید بار دیگر اقدام به ورود کنید.");
                return View(model);
            }




            var user = await _userService.GetUserByPhoneNumberAsync(model.PhoneNumber);

            if (user != null)
            {
                if (!user.IsPhoneNumberActive)
                {
                    ModelState.AddModelError("", "حساب کاربری شما فعال نمیباشد.لطفا جهت فعال سازی حساب کاربری خود بر روی لینک زیر کلیک کنید.");
                    return View(model);
                }

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        #endregion

        #region Shopper

        [Route("NewShopper")]
        [HttpGet]
        public IActionResult AddShopper()
        {
            var model = new AddOrEditShopperViewModel()
            {
                States = _stateService.GetStates() as IEnumerable<State>
            };
            

            return View(model);
        }

        [Route("NewShopper")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShopper(AddOrEditShopperViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var shopper = new Shopper()
            {
                StoreName = model.StoreName,
                BirthDay = model.BirthDay.ConvertPersianDateToEnglishDate(),
                RegisterShopper = DateTime.Now,
                LandlinePhoneNumber = model.LandlinePhoneNumber,
                Condition = false,
                IsApproved = false,
            };

            #region cards

            if (model.OnNationalCardImageName.Length > 0)
            {
                IFormFile img = model.OnNationalCardImageName;
                string imgName = NameGenerator.GenerateUniqCodeWithDash();
                string imgFileName = Path.GetExtension(img.FileName);


                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "ShoppersCardImages",
                    imgName + imgFileName);


                await using var stream = new FileStream(filePath, FileMode.Create);
                await img.CopyToAsync(stream);


                shopper.OnNationalCardImageName = imgName + imgFileName;
            }

            if (model.BackNationalCardImageName.Length > 0)
            {
                IFormFile img = model.BackNationalCardImageName;
                string imgName = NameGenerator.GenerateUniqCodeWithDash();
                string imgFileName = Path.GetExtension(img.FileName);


                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "ShoppersCardImages",
                    imgName + imgFileName);


                await using var stream = new FileStream(filePath, FileMode.Create);
                await img.CopyToAsync(stream);


                shopper.BackNationalCardImageName = imgName + imgFileName;
            }

            if (model.BusinessLicenseImageName.Length > 0)
            {
                IFormFile img = model.BusinessLicenseImageName;
                string imgName = NameGenerator.GenerateUniqCodeWithDash();
                string imgFileName = Path.GetExtension(img.FileName);


                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "ShoppersCardImages",
                    imgName + imgFileName);


                await using var stream = new FileStream(filePath, FileMode.Create);
                await img.CopyToAsync(stream);


                shopper.BusinessLicenseImageName = imgName + imgFileName;
            }

            #endregion




            var user = new Domain.Entities.User.User
            {
                ActiveCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                FullName = model.FullName,
                RegisteredDate = DateTime.Now,
                UserAvatar = "userAvatar.jpg",
                PhoneNumber = model.PhoneNumber,
                InviteCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                InviteCount = 0,
                Score = 0,
                NationalCode = model.NationalCode,
                PostalCode = model.PostalCode,
                Email = model.Email,
                IsPhoneNumberActive = false,
                IsBlocked = false,
                IsUserShopper = true,
            };


            var addShopper = await _shopperService.AddShopperAsync(user, shopper);

            if (addShopper is not null)
            {
                var addUserT0RoleResult = await _roleService.AddUserToRoleAsync(addShopper.UserId, "Shopper");

                if (addUserT0RoleResult == ResultTypes.Failed)
                {
                    ModelState.AddModelError("", "فروشنده عزیز متاسفانه هنگام ثبت نام شما به مشکلی غیر منتظره برخورد کردیم. لطفا با پشتیبانی تماس بگیرید.");
                    return View(model);
                }

                return Redirect("/");
            }
            else
            {
                ModelState.AddModelError("", "فروشنده عزیز متاسفانه هنگام ثبت نام شما به مشکلی غیر منتظره برخورد کردیم. لطفا با پشتیبانی تماس بگیرید.");
                return View(model);
            }
        }

        #endregion
    }
}
