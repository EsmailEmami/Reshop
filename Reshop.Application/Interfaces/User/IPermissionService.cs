using Reshop.Application.Enums;
using Reshop.Domain.DTOs.Permission;
using Reshop.Domain.Entities.Permission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Application.Interfaces.User
{
    public interface IPermissionService
    {
        IEnumerable<Role> GetRoles();
        Task<AddOrEditRoleViewModel> GetRoleDataAsync(string roleId);
        Task<ResultTypes> AddRoleAsync(Role role);
        Task<ResultTypes> EditRoleAsync(Role role);
        Task<Role> GetRoleByIdAsync(string roleId);
        Task<ResultTypes> RemoveRoleAsync(string roleId);
        Task<bool> IsRoleExistAsync(string roleId);

        Task<ResultTypes> AddUserToRoleAsync(string userId, string rolesName);
        Task<ResultTypes> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<ResultTypes> RemoveAllUserRolesByUserIdAsync(string userId);

        Task<ResultTypes> AddUserRoleAsync(UserRole userRole);


        #region Permission

        Task<Permission> GetPermissionByIdAsync(int permissionId);
        Task<IEnumerable<Permission>> GetPermissionsAsync();
        Task<ResultTypes> AddPermissionsToRoleAsync(string roleId, List<int> permissionsId);
        Task<ResultTypes> RemoveRolePermissionsByRoleId(string roleId);
        Task<ResultTypes> RemoveRolePermissionsByPermissionId(int permissionId);
        IEnumerable<string> GetPermissionRolesIdByPermission(string permissionName);

        Task<ResultTypes> AddPermissionAsync(Permission permission);
        Task<ResultTypes> EditPermissionAsync(Permission permission);

        bool PermissionChecker(string userId, string permissions);

        #endregion
    }
}