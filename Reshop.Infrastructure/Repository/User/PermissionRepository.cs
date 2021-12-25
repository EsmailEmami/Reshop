using Microsoft.EntityFrameworkCore;
using Reshop.Domain.Entities.Permission;
using Reshop.Domain.Interfaces.User;
using Reshop.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Infrastructure.Repository.User
{
    public class PermissionRepository : IPermissionRepository
    {
        #region constructor

        private readonly ReshopDbContext _context;

        public PermissionRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion

        #region role

        public IEnumerable<Role> GetRoles() =>
             _context.Roles;

        public async Task<Role> GetRoleByIdAsync(string roleId) => await _context.Roles.FindAsync(roleId);

        public async Task<Role> GetRoleByNameAsync(string roleName)
            =>
                await _context.Roles.SingleOrDefaultAsync(c => c.RoleTitle == roleName);

        public IEnumerable<string> GetRolesIdOfUser(string userId) =>
            _context.UserRoles
                .Where(c => c.UserId == userId)
                .Select(c => c.RoleId);

        public IEnumerable<string> GetUsersIdOfRole(string roleId) =>
            _context.UserRoles
                .Where(c => c.RoleId == roleId)
                .Select(c => c.UserId);

        public async Task AddRoleAsync(Role role) => await _context.Roles.AddAsync(role);

        public void UpdateRole(Role role) => _context.Roles.Update(role);

        public void RemoveRole(Role role) => _context.Roles.Remove(role);

        public async Task<bool> IsRoleExistAsync(string roleId) => await _context.Roles.AnyAsync(c => c.RoleId == roleId);

        #endregion

        #region user role

        public async Task<UserRole> GetUserRoleAsync(string userId, string roleId)
            =>
                await _context.UserRoles.SingleOrDefaultAsync(c => c.UserId == userId && c.RoleId == roleId);

        public IEnumerable<UserRole> GetUserRolesByUserId(string userId) =>
            _context.UserRoles
                .Where(c => c.UserId == userId);

        public IEnumerable<UserRole> GetUserRolesByRoleId(string roleId) =>
            _context.UserRoles
                .Where(c => c.RoleId == roleId);

        public async Task AddUserRoleAsync(UserRole userRole) => await _context.UserRoles.AddAsync(userRole);

        public void RemoveUserRole(UserRole userRole) => _context.UserRoles.Remove(userRole);

        public IEnumerable<Role> GetRolesOfUserWithPagination(string userId, int skip, int take, string filter)
        {
            IQueryable<Role> roles = _context.UserRoles
                .Where(c => c.UserId == userId)
                .Select(c => c.Role);

            if (!string.IsNullOrEmpty(filter))
            {
                roles = roles.Where(c => c.RoleTitle.Contains(filter));
            }

            roles = roles.Skip(skip).Take(take);

            return roles;
        }

        public async Task<int> GetUserRolesCountAsync(string userId, string filter)
        {
            IQueryable<Role> roles = _context.UserRoles
                .Where(c => c.UserId == userId)
                .Select(c => c.Role);

            if (!string.IsNullOrEmpty(filter))
            {
                roles = roles.Where(c => c.RoleTitle.Contains(filter));
            }

            return await roles.CountAsync();
        }

        public async Task<Permission> GetPermissionByIdAsync(string permissionId) =>
            await _context.Permissions.FindAsync(permissionId);

        public async Task AddPermissionAsync(Permission permission) =>
            await _context.Permissions.AddAsync(permission);

        public void UpdatePermission(Permission permission) =>
            _context.Permissions.Update(permission);

        public void RemovePermission(Permission permission) =>
            _context.Permissions.Remove(permission);

        public IEnumerable<Permission> GetPermissions() =>
            _context.Permissions;

        public async Task AddRolePermissionAsync(RolePermission rolePermission) =>
            await _context.RolePermissions.AddAsync(rolePermission);

        public IEnumerable<RolePermission> GetRolePermissionsOfRole(string roleId) =>
            _context.RolePermissions
                .Where(c => c.RoleId == roleId);

        public IEnumerable<RolePermission> GetRolePermissionsOfPermission(string permissionId) =>
            _context.RolePermissions
                .Where(c => c.PermissionId == permissionId);


        public void RemoveRolePermission(RolePermission rolePermission) =>
            _context.RolePermissions.Remove(rolePermission);

        public IEnumerable<string> GetPermissionsIdOfRole(string roleId) =>
            _context.RolePermissions.Where(c => c.RoleId == roleId)
                .Select(c => c.PermissionId);

        public async Task<string> GetPermissionIdByNameAsync(string permissionName) =>
            await _context.Permissions.Where(c => c.PermissionTitle == permissionName)
                    .Select(c => c.PermissionId)
                    .SingleOrDefaultAsync();

        public IEnumerable<string> GetRolesIdOfPermission(string permissionId) =>
            _context.RolePermissions
                .Where(c => c.PermissionId == permissionId)
                .Select(c => c.RoleId);

        public async Task<bool> IsPermissionExistsAsync(string permissionId) =>
            await _context.Permissions.AnyAsync(c => c.PermissionId == permissionId);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        #endregion
    }
}