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

        private readonly IUserService _userService;

        public RoleManagerController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion


        [HttpGet]
        public IActionResult Index()
        {
            return View(_userService.GetRoles());
        }

        [HttpGet]
        public async Task<IActionResult> AddOrEditRole(string roleId = "")
        {
            if (roleId == "")
            {
                return View();
            }
            else
            {
                if (!await _userService.IsRoleExistAsync(roleId)) return NotFound();

                return View(await _userService.GetRoleDataAsync(roleId));
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

                await _userService.AddRoleAsync(role);
            }
            else
            {
                var role = await _userService.GetRoleByIdAsync(model.RoleId);

                if (role == null) return NotFound();

                role.RoleTitle = model.RoleTitle;

                await _userService.EditRoleAsync(role);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(string roleId)
        {
            if (!await _userService.IsRoleExistAsync(roleId)) return NotFound();

            await _userService.RemoveRoleAsync(roleId);

            return RedirectToAction("Index");
        }
    }
}
