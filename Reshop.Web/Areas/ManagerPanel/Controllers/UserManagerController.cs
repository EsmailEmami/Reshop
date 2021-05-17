using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Generator;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;
using System;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Application.Enums;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class UserManagerController : Controller
    {
        #region constructor

        private readonly IUserService _userService;

        public UserManagerController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        [HttpGet]
        public IActionResult Index()
        {
            return View(_userService.GetUsersInformation());
        }

        #region add or edit

        [HttpGet]
        public async Task<IActionResult> AddOrEditUser(string userId = "")
        {
            if (userId == "")
            {
                var model = new AddOrEditUserViewModel()
                {
                    Roles = _userService.GetRoles()
                };
                return View(model);
            }
            else
            {
                if (!await _userService.IsUserExistAsync(userId)) return NotFound();

                return View(await _userService.GetUserDataAsync(userId));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditUser(AddOrEditUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (string.IsNullOrEmpty(model.UserId))
            {
                var user = new User
                {
                    ActiveCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                    FullName = model.FullName,
                    RegisteredDate = DateTime.Now,
                    UserAvatar = "userAvatar.jpg",
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    InviteCode = NameGenerator.GenerateUniqUpperCaseCodeWithoutDash(),
                    InviteCount = 0,
                    Score = 0,
                    NationalCode = model.NationalCode,
                    PostalCode = model.PostalCode,
                    IsPhoneNumberActive = model.IsPhoneNumberActive,
                    IsBlocked = model.IsBlocked,
                    AccountBalance = model.AccountBalance,
                };

                var addUser = await _userService.AddUserAsync(user);

                if (addUser == ResultTypes.Successful)
                {
                    if (model.SelectedRoles != null )
                    {
                        foreach (var role in model.SelectedRoles)
                        {
                            var userRole = new UserRole()
                            {
                                UserId = user.UserId,
                                RoleId = role
                            };

                            await _userService.AddUserRoleAsync(userRole);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("","عنگام ثبت کاربر مشکلی غیر منتظره به وجود آمده است.");
                    return View(model);
                }
            }
            else
            {
                var user = await _userService.GetUserByIdAsync(model.UserId);

                if (user == null) return NotFound();


                user.FullName = model.FullName;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.InviteCount = 0;
                user.Score = 0;
                user.NationalCode = model.NationalCode;
                user.PostalCode = model.PostalCode;
                user.IsPhoneNumberActive = model.IsPhoneNumberActive;
                user.IsBlocked = model.IsBlocked;
                user.AccountBalance = model.AccountBalance;


                var editUser = await _userService.EditUserAsync(user);

                if (editUser == ResultTypes.Successful)
                {
                    // remove user roles
                    await _userService.RemoveUserRolesByUserIdAsync(user.UserId);


                    if (model.SelectedRoles != null)
                    {
                        foreach (var roleId in model.SelectedRoles)
                        {
                            var userRole = new UserRole()
                            {
                                UserId = user.UserId,
                                RoleId = roleId
                            };
                            await _userService.AddUserRoleAsync(userRole);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "عنگام ثبت کاربر مشکلی غیر منتظره به وجود آمده است.");
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region remove

        [HttpPost]
        [ValidateAntiForgeryToken]
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
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            return View(user);
        }

        #endregion
    }
}
