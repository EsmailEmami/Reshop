using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;
using System.Threading.Tasks;
using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Security.Attribute;
using Reshop.Domain.Entities.Permission;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    [AutoValidateAntiforgeryToken]
    public class RoleManagerController : Controller
    {
        #region constructor

        private readonly IRoleService _roleService;

        public RoleManagerController(IRoleService roleService)
        {
            _roleService = roleService;
        }


        #endregion


        [HttpGet]
        [Permission("RoleManager")]
        public IActionResult Index()
        {
            return View(_roleService.GetRoles());
        }

        [HttpGet]
        [Permission("AddRole,EditRole")]
        public async Task<IActionResult> AddOrEditRole(string roleId = "")
        {
            if (roleId == "")
            {
                var model = new AddOrEditRoleViewModel()
                {
                    RoleId = "",
                    Permissions = _roleService.GetPermissions() as IEnumerable<Permission>
                };

                return View(model);
            }
            else
            {
                if (!await _roleService.IsRoleExistAsync(roleId)) return NotFound();

                return View(await _roleService.GetRoleDataAsync(roleId));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditRole(AddOrEditRoleViewModel model)
        {
            model.Permissions = _roleService.GetPermissions() as IEnumerable<Permission>;
            if (!ModelState.IsValid) return View(model);

            if (string.IsNullOrEmpty(model.RoleId))
            {
                var role = new Role()
                {
                    RoleTitle = Fixer.FixedText(model.RoleTitle),
                };

                var addRole = await _roleService.AddRoleAsync(role);

                if (addRole == ResultTypes.Successful)
                {
                    if (model.SelectedPermissions.Any())
                    {
                        var addRolePermission = await _roleService.AddPermissionsToRoleAsync(role.RoleId, model.SelectedPermissions as List<int>);
                        if (addRolePermission == ResultTypes.Failed)
                        {
                            ModelState.AddModelError("", "هنگام افزودن دسترسی ها به مشکل خوردیم.");
                            return View(model);
                        }
                    }

                    return RedirectToAction("Index");
                }


                ModelState.AddModelError("", "هنگام افزودن مقام به مشکل خوردیم");
                return View(model);
            }
            else
            {
                var role = await _roleService.GetRoleByIdAsync(model.RoleId);

                if (role == null) return NotFound();

                role.RoleTitle = model.RoleTitle;

                var editRole = await _roleService.EditRoleAsync(role);

                if (editRole == ResultTypes.Failed)
                {
                    ModelState.AddModelError("", "هنگام ویرایش مقام به مشکل خوردیم");
                    return View(model);
                }


                var removeRole = await _roleService.RemoveRolePermissionsByRoleId(role.RoleId);
                if (removeRole == ResultTypes.Failed)
                {
                    ModelState.AddModelError("", "هنگام ویرایش دسترسی ها به مشکل برخوردیم");
                    return View(model);
                }


                if (model.SelectedPermissions.Any())
                {
                    var addRolePermission = await _roleService.AddPermissionsToRoleAsync(role.RoleId, model.SelectedPermissions as List<int>);
                    if (addRolePermission == ResultTypes.Failed)
                    {
                        ModelState.AddModelError("", "هنگام ویرایش دسترسی ها به مشکل برخوردیم");
                        return View(model);
                    }
                }

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("AddRole,EditRole")]
        public async Task<IActionResult> RemoveRole(string roleId)
        {
            if (!await _roleService.IsRoleExistAsync(roleId)) return NotFound();

            await _roleService.RemoveRoleAsync(roleId);

            return RedirectToAction("Index");
        }
    }
}
