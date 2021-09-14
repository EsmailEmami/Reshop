using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Permission;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.Interfaces.User
{
    public interface IRoleRepository
    {
        #region role

        IEnumerable<Role> GetRoles();
        Task<Role> GetRoleByIdAsync(string roleId);
        Task<Role> GetRoleByNameAsync(string roleName);
        IEnumerable<string> GetRolesIdOfUser(string userId);
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

        #endregion

        #region permission

        IEnumerable<Permission> GetPermissions();
        Task AddRolePermissionAsync(RolePermission rolePermission);
        IAsyncEnumerable<RolePermission> GetRolePermissionsOfRole(string roleId);
        void RemoveRolePermission(RolePermission rolePermission);
        IEnumerable<int> GetPermissionsIdOfRole(string roleId);
        int GetPermissionIdByName(string permissionName);
        IEnumerable<string> GetRolesIdOfPermission(int permissionId);

        #endregion

        Task SaveChangesAsync();
    }
}