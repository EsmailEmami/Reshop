using Reshop.Domain.Entities.Permission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Domain.Interfaces.User
{
    public interface IPermissionRepository
    {
        #region role

        IEnumerable<Role> GetRoles();
        Task<Role> GetRoleByIdAsync(string roleId);
        Task<Role> GetRoleByNameAsync(string roleName);
        IEnumerable<string> GetRolesIdOfUser(string userId);
        IEnumerable<string> GetUsersIdOfRole(string roleId);
        Task AddRoleAsync(Role role);
        void UpdateRole(Role role);
        void RemoveRole(Role role);
        Task<bool> IsRoleExistAsync(string roleId);

        #endregion

        #region user role

        Task<UserRole> GetUserRoleAsync(string userId, string roleId);
        IEnumerable<UserRole> GetUserRolesByUserId(string userId);
        IEnumerable<UserRole> GetUserRolesByRoleId(string roleId);
        Task AddUserRoleAsync(UserRole userRole);
        void RemoveUserRole(UserRole userRole);

        IEnumerable<Role> GetRolesOfUserWithPagination(string userId, int skip, int take, string filter);
        Task<int> GetUserRolesCountAsync(string userId, string filter);

        #endregion

        #region permission

        Task<Permission> GetPermissionByIdAsync(string permissionId);
        Task AddPermissionAsync(Permission permission);
        void UpdatePermission(Permission permission);
        void RemovePermission(Permission permission);
        IEnumerable<Permission> GetPermissions();
        Task AddRolePermissionAsync(RolePermission rolePermission);
        IEnumerable<RolePermission> GetRolePermissionsOfRole(string roleId);
        IEnumerable<RolePermission> GetRolePermissionsOfPermission(string permissionId);
        void RemoveRolePermission(RolePermission rolePermission);
        IEnumerable<string> GetPermissionsIdOfRole(string roleId);
        Task<string> GetPermissionIdByNameAsync(string permissionName);
        IEnumerable<string> GetRolesIdOfPermission(string permissionId);
        Task<bool> IsPermissionExistsAsync(string permissionId);

        #endregion

        Task SaveChangesAsync();
    }
}