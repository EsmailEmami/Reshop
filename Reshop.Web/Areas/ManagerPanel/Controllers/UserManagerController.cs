using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Generator;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Permission;
using Reshop.Domain.Entities.User;
using System;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Application.Attribute;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    public class UserManagerController : Controller
    {
        #region constructor

        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;

        public UserManagerController(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View(_userService.GetUsersInformation());
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
                    Roles = _permissionService.GetRoles()
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


                return View();
            }
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditUser(AddOrEditUserForAdminViewModel model)
        {
            model.Roles = _permissionService.GetRoles();

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
                    if (model.SelectedRoles != null)
                    {
                        foreach (var role in model.SelectedRoles)
                        {
                            var userRole = new UserRole()
                            {
                                UserId = user.UserId,
                                RoleId = role
                            };

                            await _permissionService.AddUserRoleAsync(userRole);
                        }
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
                    var removeUserRoles = await _permissionService.RemoveAllUserRolesByUserIdAsync(user.UserId);

                    if (removeUserRoles != ResultTypes.Successful)
                    {
                        ModelState.AddModelError("", "هنگام ویراش مقام کاربر مشکلی غیر منتظره پیش آمده است! لطفا دوباره تلاش کنید.");
                        return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                    }

                    if (model.SelectedRoles.Any())
                    {
                        foreach (var roleId in model.SelectedRoles)
                        {
                            var userRole = new UserRole()
                            {
                                UserId = user.UserId,
                                RoleId = roleId
                            };
                            await _permissionService.AddUserRoleAsync(userRole);
                        }
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

        [HttpGet]
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
    }
}
