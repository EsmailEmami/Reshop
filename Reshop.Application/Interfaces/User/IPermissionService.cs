using System;
using Reshop.Application.Enums;
using Reshop.Domain.DTOs.Permission;
using Reshop.Domain.Entities.Permission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Application.Interfaces.User
{
    public interface IPermissionService
    {
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<AddOrEditRoleViewModel> GetRoleDataAsync(string roleId);
        Task<ResultTypes> AddRoleAsync(Role role);
        Task<ResultTypes> EditRoleAsync(Role role);
        Task<Role> GetRoleByIdAsync(string roleId);
        Task<ResultTypes> DeleteRoleAsync(string roleId);
        Task<bool> IsRoleExistAsync(string roleId);

        Task<ResultTypes> AddUserToRoleAsync(string userId, params string[] rolesName);
        Task<ResultTypes> AddUserToRoleAsync(string userId, List<string> rolesId);
        Task<ResultTypes> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<ResultTypes> DeleteAllUserRolesByUserIdAsync(string userId);

        Task<ResultTypes> AddUserRoleAsync(UserRole userRole);


        Task<Tuple<IEnumerable<Role>, int, int>> GetRolesOfUserWithPaginationAsync(string userId, int pageId = 1, int take = 15, string filter = null);


        #region Permission

        Task<Permission> GetPermissionByIdAsync(string permissionId);
        Task<IEnumerable<Permission>> GetPermissionsAsync();
        Task<AddOrEditPermissionViewModel> GetPermissionDataAsync(string permissionId);
        Task<ResultTypes> AddRolePermissionByRoleAsync(string roleId, IEnumerable<string> permissionsId);
        Task<ResultTypes> AddRolePermissionByPermissionAsync(string permissionId, List<string> rolesId);
        Task<ResultTypes> RemoveRolePermissionsByRoleId(string roleId);
        Task<ResultTypes> RemoveRolePermissionsByPermissionId(string permissionId);
        Task<bool> IsPermissionExistsAsync(string permissionId);
        Task<ResultTypes> DeletePermissionAsync(string permissionId);
        Task<ResultTypes> AddPermissionAsync(Permission permission);
        Task<ResultTypes> EditPermissionAsync(Permission permission);

        Task<bool> PermissionCheckerAsync(string userId, params string[] permissions);

        #endregion
    }
}