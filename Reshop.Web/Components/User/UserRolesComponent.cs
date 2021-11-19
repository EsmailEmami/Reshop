using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reshop.Application.Interfaces.User;

namespace Reshop.Web.Components.User
{
    public class UserRolesComponent : ViewComponent
    {
        private readonly IPermissionService _permissionService;

        public UserRolesComponent(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId, int pageId = 1, string filter = null)
        {
            int take = 20;

            var roles = await _permissionService.GetRolesOfUserWithPaginationAsync(userId, pageId, take, filter);

            ViewBag.SearchText = filter;
            ViewBag.TakeCount = take;
            ViewBag.UserId = userId;

            return View("/Views/Shared/Components/User/UserRoles.cshtml", roles);
        }
    }
}
