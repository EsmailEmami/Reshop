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
using Reshop.Application.Security;
using Reshop.Application.Security.GoogleRecaptcha;
using Reshop.Application.Senders;
using Reshop.Application.Validations.Google;
using Reshop.Domain.DTOs.Shopper;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Shopper;
using Reshop.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reshop.Web.Controllers.User
{
    public class AccountController : Controller
    {
        #region constructor

        private readonly IUserService _userService;

        private readonly IMessageSender _messageSender;
        private readonly IOptions<GoogleReCaptchaKey> _captchaKey;
        private readonly IShopperService _shopperService;
        private readonly IRoleService _roleService;
        private readonly IStateService _stateService;

        public AccountController(IUserService userService, IMessageSender messageSender, IOptions<GoogleReCaptchaKey> captchaKey, IShopperService shopperService, IRoleService roleService, IStateService stateService)
        {
            _userService = userService;
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
                ActiveCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                FullName = model.FullName,
                RegisteredDate = DateTime.Now,
                UserAvatar = "userAvatar.jpg",
                PhoneNumber = model.PhoneNumber,
                InviteCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                InviteCount = 0,
                Score = 0,
                NationalCode = "-",
                IsPhoneNumberActive = false,
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
            var model = new AddShopperViewModel()
            {
                States = _stateService.GetStates() as IEnumerable<State>,
                StoreTitles = _shopperService.GetStoreTitles(),
            };


            return View(model);
        }

        [Route("NewShopper")]
        [HttpPost]
        public async Task<IActionResult> AddShopper(AddShopperViewModel model)
        {
            // this is states when page reload states is not null
            model.States = _stateService.GetStates() as IEnumerable<State>;
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
                    ActiveCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                    RegisteredDate = DateTime.Now,
                    UserAvatar = "userAvatar.jpg",
                    PhoneNumber = model.PhoneNumber,
                    InviteCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                    InviteCount = 0,
                    Score = 0,
                    NationalCode = model.NationalCode,
                    Email = model.Email,
                    IsPhoneNumberActive = true,
                    IsBlocked = false
                };

                var addUser = await _userService.AddUserAsync(user);

                if (addUser == ResultTypes.Successful)
                {
                    var addUserToRoleResult = await _roleService.AddUserToRoleAsync(user.UserId, "Shopper");

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
                        StateId = model.StateId,
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
    }
}
