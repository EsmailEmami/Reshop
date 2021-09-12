using System;
using Microsoft.EntityFrameworkCore;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using Reshop.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Permission;

namespace Reshop.Infrastructure.Repository.User
{
    public class RoleRepository : IRoleRepository
    {
        #region constructor

        private readonly ReshopDbContext _context;

        public RoleRepository(ReshopDbContext context)
        {
            _context = context;
        }

        #endregion

        #region role

        public IAsyncEnumerable<Role> GetRoles() => _context.Roles;

        public async Task<Role> GetRoleByIdAsync(string roleId) => await _context.Roles.FindAsync(roleId);

        public async Task<Role> GetRoleByNameAsync(string roleName)
            =>
                await _context.Roles.SingleOrDefaultAsync(c => c.RoleTitle == roleName);

        public IEnumerable<string> GetRolesIdOfUser(string userId) =>
                _context.UserRoles.Where(c => c.UserId == userId)
                    .Select(c => c.RoleId);

        public IAsyncEnumerable<string> GetUsersIdOfRole(string roleId)
            =>
                _context.UserRoles.Where(c => c.RoleId == roleId)
                    .Select(c => c.UserId) as IAsyncEnumerable<string>;

        public async Task AddRoleAsync(Role role) => await _context.Roles.AddAsync(role);

        public void UpdateRole(Role role) => _context.Roles.Update(role);

        public void RemoveRole(Role role) => _context.Roles.Remove(role);

        public async Task<bool> IsRoleExistAsync(string roleId) => await _context.Roles.AnyAsync(c => c.RoleId == roleId);

        #endregion

        #region user role

        public async Task<UserRole> GetUserRoleAsync(string userId, string roleId)
            =>
                await _context.UserRoles.SingleOrDefaultAsync(c => c.UserId == userId && c.RoleId == roleId);

        public IAsyncEnumerable<UserRole> GetUserRolesByUserId(string userId)
            =>
                _context.UserRoles.Where(c => c.UserId == userId) as IAsyncEnumerable<UserRole>;

        public IAsyncEnumerable<UserRole> GetUserRolesByRoleId(string roleId)
            =>
                _context.UserRoles.Where(c => c.RoleId == roleId) as IAsyncEnumerable<UserRole>;

        public async Task AddUserRoleAsync(UserRole userRole) => await _context.UserRoles.AddAsync(userRole);

        public void RemoveUserRole(UserRole userRole) => _context.UserRoles.Remove(userRole);

        public IAsyncEnumerable<Permission> GetPermissions() => _context.Permissions;

        public async Task AddRolePermissionAsync(RolePermission rolePermission) =>
            await _context.RolePermissions.AddAsync(rolePermission);

        public IAsyncEnumerable<RolePermission> GetRolePermissionsOfRole(string roleId) =>
            _context.RolePermissions.Where(c => c.RoleId == roleId) as IAsyncEnumerable<RolePermission>;

        public void RemoveRolePermission(RolePermission rolePermission) =>
            _context.RolePermissions.Remove(rolePermission);

        public IAsyncEnumerable<int> GetPermissionsIdOfRole(string roleId) =>
            _context.RolePermissions.Where(c => c.RoleId == roleId)
                .Select(c => c.PermissionId) as IAsyncEnumerable<int>;

        public int GetPermissionIdByName(string permissionName)
        {
            try
            {
               return _context.Permissions.First(c => c.PermissionTitle == permissionName).PermissionId;
            }
            catch
            {
                return 0;
            }
        }

        public IAsyncEnumerable<string> GetRolesIdOfPermission(int permissionId) =>
            _context.RolePermissions.Where(c => c.PermissionId == permissionId)
                .Select(c => c.RoleId) as IAsyncEnumerable<string>;


        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        #endregion
    }
}