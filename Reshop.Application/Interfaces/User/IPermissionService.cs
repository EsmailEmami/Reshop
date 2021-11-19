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

        Task<ResultTypes> AddUserToRoleAsync(string userId, string rolesName);
        Task<ResultTypes> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<ResultTypes> RemoveAllUserRolesByUserIdAsync(string userId);

        Task<ResultTypes> AddUserRoleAsync(UserRole userRole);


        Task<Tuple<IEnumerable<Role>, int, int>> GetRolesOfUserWithPaginationAsync(string userId, int pageId = 1, int take = 15, string filter = null);


        #region Permission

        Task<Permission> GetPermissionByIdAsync(int permissionId);
        Task<IEnumerable<Permission>> GetPermissionsAsync();
        Task<AddOrEditPermissionViewModel> GetPermissionDataAsync(int permissionId);
        Task<ResultTypes> AddRolePermissionByRoleAsync(string roleId, List<int> permissionsId);
        Task<ResultTypes> AddRolePermissionByPermissionAsync(int permissionId, List<string> rolesId);
        Task<ResultTypes> RemoveRolePermissionsByRoleId(string roleId);
        Task<ResultTypes> RemoveRolePermissionsByPermissionId(int permissionId);
        Task<bool> IsPermissionExistsAsync(int permissionId);
        Task<ResultTypes> DeletePermissionAsync(int permissionId);
        Task<ResultTypes> AddPermissionAsync(Permission permission);
        Task<ResultTypes> EditPermissionAsync(Permission permission);

        Task<bool> PermissionCheckerAsync(string userId, params string[] permissions);

        #endregion
    }
}