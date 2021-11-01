using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.Permission;
using Reshop.Domain.Entities.Permission;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    public class PermissionManagerController : Controller
    {
        #region constructor

        private readonly IPermissionService _permissionService;

        public PermissionManagerController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }


        #endregion

        #region role

        [HttpGet]
        public async Task<IActionResult> RoleIndex()
        {
            return View(await _permissionService.GetRolesAsync());
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditRole(string roleId = "")
        {
            if (roleId == "")
            {
                var model = new AddOrEditRoleViewModel()
                {
                    Permissions = await _permissionService.GetPermissionsAsync()
                };

                return View(model);
            }
            else
            {
                if (!await _permissionService.IsRoleExistAsync(roleId))
                    return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

                return View(await _permissionService.GetRoleDataAsync(roleId));
            }
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditRole(AddOrEditRoleViewModel model)
        {
            model.Permissions = await _permissionService.GetPermissionsAsync();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            if (string.IsNullOrEmpty(model.RoleId))
            {
                var role = new Role()
                {
                    RoleTitle = model.RoleTitle.FixedText(),
                };

                var addRole = await _permissionService.AddRoleAsync(role);

                if (addRole == ResultTypes.Successful)
                {
                    if (model.SelectedPermissions.Any())
                    {
                        var addRolePermission = await _permissionService.AddRolePermissionByRoleAsync(role.RoleId, model.SelectedPermissions as List<int>);
                        if (addRolePermission == ResultTypes.Failed)
                        {
                            ModelState.AddModelError("", "هنگام افزودن دسترسی ها به مشکل خوردیم.");
                            return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                        }
                    }

                    return Json(new { isValid = true, returnUrl = "current" });
                }


                ModelState.AddModelError("", "هنگام افزودن مقام به مشکل خوردیم");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
            else
            {
                var role = await _permissionService.GetRoleByIdAsync(model.RoleId);

                if (role == null) return NotFound();

                role.RoleTitle = model.RoleTitle;

                var editRole = await _permissionService.EditRoleAsync(role);

                if (editRole == ResultTypes.Failed)
                {
                    ModelState.AddModelError("", "هنگام ویرایش مقام به مشکل خوردیم");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                }


                var removeRole = await _permissionService.RemoveRolePermissionsByRoleId(role.RoleId);
                if (removeRole == ResultTypes.Failed)
                {
                    ModelState.AddModelError("", "هنگام ویرایش دسترسی ها به مشکل برخوردیم");
                    return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                }


                if (model.SelectedPermissions.Any())
                {
                    var addRolePermission = await _permissionService.AddRolePermissionByRoleAsync(role.RoleId, model.SelectedPermissions as List<int>);
                    if (addRolePermission == ResultTypes.Failed)
                    {
                        ModelState.AddModelError("", "هنگام ویرایش دسترسی ها به مشکل برخوردیم");
                        return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
                    }
                }

                return Json(new { isValid = true, returnUrl = "current" });
            }
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> RemoveRole(string roleId)
        {
            if (!await _permissionService.IsRoleExistAsync(roleId))
                return NotFound();

            await _permissionService.DeleteRoleAsync(roleId);

            return RedirectToAction("RoleIndex");
        }

        #endregion

        #region permission

        public async Task<IActionResult> PermissionIndex()
        {
            return View(await _permissionService.GetPermissionsAsync());
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> AddPermission()
        {
            var model = new AddOrEditPermissionViewModel()
            {
                Roles = await _permissionService.GetRolesAsync(),
                Permissions = await _permissionService.GetPermissionsAsync()
            };

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> AddPermission(AddOrEditPermissionViewModel model)
        {
            model.Roles = await _permissionService.GetRolesAsync();
            model.Permissions = await _permissionService.GetPermissionsAsync();

            if (!ModelState.IsValid)
                return Json(new
                {
                    isValid = false,
                    html = RenderViewToString.RenderRazorViewToString(this, model)
                });

            var permission = new Permission()
            {
                PermissionTitle = model.PermissionTitle,
            };

            if (model.ParentId > 0)
            {
                permission.ParentId = model.ParentId;
            }

            var res = await _permissionService.AddPermissionAsync(permission);

            if (res == ResultTypes.Successful)
            {
                await _permissionService.AddRolePermissionByPermissionAsync(permission.PermissionId, model.SelectedRoles.ToList());


                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                ModelState.AddModelError("", "هنگام افزودن دسترسی ها به مشکل برخوردیم");
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });
            }
        }

        [HttpGet]
        [NoDirectAccess]
        public async Task<IActionResult> EditPermission(int permissionId)
        {
            var permission = await _permissionService.GetPermissionDataAsync(permissionId);

            if (permission == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            return View(permission);
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> EditPermission(AddOrEditPermissionViewModel model)
        {
            model.Roles = await _permissionService.GetRolesAsync();
            model.Permissions = await _permissionService.GetPermissionsAsync();

            if (!ModelState.IsValid)
                return Json(new
                {
                    isValid = false,
                    html = RenderViewToString.RenderRazorViewToString(this, model)
                });

            var permission = await _permissionService.GetPermissionByIdAsync(model.PermissionId);

            if (permission == null)
            {
                ModelState.AddModelError("", "هنگام ویرایش دسترسی ها به مشکل برخوردیم");
                return Json(new
                {
                    isValid = false,
                    html = RenderViewToString.RenderRazorViewToString(this, model)
                });
            }

            permission.ParentId = model.ParentId;
            permission.PermissionTitle = model.PermissionTitle;

            var res = await _permissionService.EditPermissionAsync(permission);

            if (res == ResultTypes.Successful)
            {
                await _permissionService.RemoveRolePermissionsByPermissionId(permission.PermissionId);

                await _permissionService.AddRolePermissionByPermissionAsync(permission.PermissionId, model.SelectedRoles.ToList());

                return Json(new { isValid = true, returnUrl = "current" });
            }
            else
            {
                ModelState.AddModelError("", "هنگام ویرایش دسترسی ها به مشکل برخوردیم");
                return Json(new
                {
                    isValid = false,
                    html = RenderViewToString.RenderRazorViewToString(this, model)
                });
            }
        }

        [HttpPost]
        [NoDirectAccess]
        public async Task<IActionResult> DeletePermission(int permissionId)
        {
            if (await _permissionService.IsPermissionExistsAsync(permissionId))
                return NotFound();

            var delete = await _permissionService.DeletePermissionAsync(permissionId);

            if (delete == ResultTypes.Successful)
            {
                return RedirectToAction("PermissionIndex");
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

    }
}
