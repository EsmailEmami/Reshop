using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;
using System.Threading.Tasks;

namespace Reshop.Web.Areas.ManagerPanel.Controllers
{
    [Area("ManagerPanel")]
    [Authorize]
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
        public IActionResult Index()
        {
            return View(_roleService.GetRoles());
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditRole(string roleId = "")
        {
            if (roleId == "")
            {
                return View(new AddOrEditRoleViewModel() { RoleId = "" });
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
            if (!ModelState.IsValid) return View(model);

            if (string.IsNullOrEmpty(model.RoleId))
            {
                var role = new Role()
                {
                    RoleTitle = model.RoleTitle,
                };

                await _roleService.AddRoleAsync(role);
            }
            else
            {
                var role = await _roleService.GetRoleByIdAsync(model.RoleId);

                if (role == null) return NotFound();

                role.RoleTitle = model.RoleTitle;

                await _roleService.EditRoleAsync(role);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(string roleId)
        {
            if (!await _roleService.IsRoleExistAsync(roleId)) return NotFound();

            await _roleService.RemoveRoleAsync(roleId);

            return RedirectToAction("Index");
        }
    }
}
