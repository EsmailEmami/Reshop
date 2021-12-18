using Reshop.Domain.Entities.Permission;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reshop.Domain.Interfaces.User
{
    public interface IPermissionRepository
    {
        #region role

        Task<IEnumerable<Role>> GetRolesAsync();
        Task<Role> GetRoleByIdAsync(string roleId);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<IEnumerable<string>> GetRolesIdOfUserAsync(string userId);
        IAsyncEnumerable<string> GetUsersIdOfRole(string roleId);
        Task AddRoleAsync(Role role);
        void UpdateRole(Role role);
        void RemoveRole(Role role);
        Task<bool> IsRoleExistAsync(string roleId);

        #endregion

        #region user role

        Task<UserRole> GetUserRoleAsync(string userId, string roleId);
        IAsyncEnumerable<UserRole> GetUserRolesByUserId(string userId);
        IAsyncEnumerable<UserRole> GetUserRolesByRoleId(string roleId);
        Task AddUserRoleAsync(UserRole userRole);
        void RemoveUserRole(UserRole userRole);

        Task<IEnumerable<Role>> GetRolesOfUserWithPaginationAsync(string userId, int skip, int take, string filter);
        Task<int> GetUserRolesCountAsync(string userId, string filter);

        #endregion

        #region permission

        Task<Permission> GetPermissionByIdAsync(string permissionId);
        Task AddPermissionAsync(Permission permission);
        void UpdatePermission(Permission permission);
        void RemovePermission(Permission permission);
        Task<IEnumerable<Permission>> GetPermissionsAsync();
        Task AddRolePermissionAsync(RolePermission rolePermission);
        Task<IEnumerable<RolePermission>> GetRolePermissionsOfRoleAsync(string roleId);
        Task<IEnumerable<RolePermission>> GetRolePermissionsOfPermissionAsync(string permissionId);
        void RemoveRolePermission(RolePermission rolePermission);
        IEnumerable<string> GetPermissionsIdOfRole(string roleId);
        Task<string> GetPermissionIdByNameAsync(string permissionName);
        Task<IEnumerable<string>> GetRolesIdOfPermissionAsync(string permissionId);
        Task<bool> IsPermissionExistsAsync(string permissionId);

        #endregion

        Task SaveChangesAsync();
    }
}