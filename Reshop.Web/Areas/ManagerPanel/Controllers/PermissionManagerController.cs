using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Attribute;
using Reshop.Application.Constants;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.User;
using Reshop.Application.Security.Attribute;
using Reshop.Domain.DTOs.Permission;
using Reshop.Domain.Entities.Permission;
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
        [Permission(PermissionsConstants.RolesPage)]
        public IActionResult RoleIndex()
        {
            return View(_permissionService.GetRoles());
        }

        [HttpGet]
        [NoDirectAccess]
        [PermissionJs(false, PermissionsConstants.AddRole, PermissionsConstants.EditRole)]
        public async Task<IActionResult> AddOrEditRole(string roleId = "")
        {
            if (roleId == "")
            {
                var model = new AddOrEditRoleViewModel()
                {
                    Permissions = _permissionService.GetPermissions()
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
        [PermissionJs(false, PermissionsConstants.AddRole, PermissionsConstants.EditRole)]
        public async Task<IActionResult> AddOrEditRole(AddOrEditRoleViewModel model)
        {
            model.Permissions = _permissionService.GetPermissions();

            if (!ModelState.IsValid)
                return Json(new { isValid = false, html = RenderViewToString.RenderRazorViewToString(this, model) });

            if (string.IsNullOrEmpty(model.RoleId))
            {
                var role = new Role()
                {
                    RoleTitle = model.RoleTitle,
                };

                var addRole = await _permissionService.AddRoleAsync(role);

                if (addRole == ResultTypes.Successful)
                {
                    if (model.SelectedPermissions != null && model.SelectedPermissions.Any())
                    {
                        var addRolePermission = await _permissionService.AddRolePermissionByRoleAsync(role.RoleId, model.SelectedPermissions);
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


                if (model.SelectedPermissions != null && model.SelectedPermissions.Any())
                {
                    var addRolePermission = await _permissionService.AddRolePermissionByRoleAsync(role.RoleId, model.SelectedPermissions);
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
        [Permission(PermissionsConstants.DeleteRole)]
        public async Task<IActionResult> RemoveRole(string roleId)
        {
            if (!await _permissionService.IsRoleExistAsync(roleId))
                return NotFound();

            await _permissionService.DeleteRoleAsync(roleId);

            return RedirectToAction("RoleIndex");
        }

        #endregion

        #region permission

        [Permission(PermissionsConstants.PermissionsPage)]
        public IActionResult PermissionIndex()
        {
            return View(_permissionService.GetPermissions());
        }

        [HttpGet]
        [NoDirectAccess]
        [PermissionJs(false, PermissionsConstants.AddPermission)]
        public IActionResult AddPermission()
        {
            var model = new AddOrEditPermissionViewModel()
            {
                Roles = _permissionService.GetRoles(),
                Permissions = _permissionService.GetPermissions()
            };

            return View(model);
        }

        [HttpPost]
        [NoDirectAccess]
        [PermissionJs(false, PermissionsConstants.AddPermission)]
        public async Task<IActionResult> AddPermission(AddOrEditPermissionViewModel model)
        {
            model.Roles = _permissionService.GetRoles();
            model.Permissions = _permissionService.GetPermissions();

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

            if (!string.IsNullOrEmpty(model.ParentId))
            {
                if (model.ParentId != "0")
                {
                    permission.ParentId = model.ParentId;
                }
            }

            var res = await _permissionService.AddPermissionAsync(permission);

            if (res == ResultTypes.Successful)
            {
                if (model.SelectedRoles != null && model.SelectedRoles.Any())
                {
                    await _permissionService.AddRolePermissionByPermissionAsync(permission.PermissionId, model.SelectedRoles.ToList());
                }

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
        [PermissionJs(false, PermissionsConstants.EditPermission)]
        public async Task<IActionResult> EditPermission(string permissionId)
        {
            var permission = await _permissionService.GetPermissionDataAsync(permissionId);

            if (permission == null)
                return Json(new { isValid = false, errorType = "danger", errorText = "مشکلی پیش آمده است! لطفا دوباره تلاش کنید." });

            return View(permission);
        }

        [HttpPost]
        [NoDirectAccess]
        [PermissionJs(false, PermissionsConstants.EditPermission)]
        public async Task<IActionResult> EditPermission(AddOrEditPermissionViewModel model)
        {
            model.Roles = _permissionService.GetRoles();
            model.Permissions = _permissionService.GetPermissions();

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

            permission.PermissionTitle = model.PermissionTitle;
            permission.ParentId = null;

            if (!string.IsNullOrEmpty(model.ParentId))
            {
                permission.ParentId = model.ParentId;
            }

            var res = await _permissionService.EditPermissionAsync(permission);

            if (res == ResultTypes.Successful)
            {
                await _permissionService.RemoveRolePermissionsByPermissionId(permission.PermissionId);

                if (model.SelectedRoles != null && model.SelectedRoles.Any())
                {
                    await _permissionService.AddRolePermissionByPermissionAsync(permission.PermissionId, model.SelectedRoles.ToList());
                }

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
        [Permission(PermissionsConstants.DeletePermission)]
        public async Task<IActionResult> DeletePermission(string permissionId)
        {
            if (!await _permissionService.IsPermissionExistsAsync(permissionId))
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
