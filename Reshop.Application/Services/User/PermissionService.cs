using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.Permission;
using Reshop.Domain.Entities.Permission;
using Reshop.Domain.Interfaces.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Application.Services.User
{
    public class PermissionService : IPermissionService
    {
        #region constructor

        private readonly IPermissionRepository _permissionRepository;
        private readonly IUserRepository _userRepository;

        public PermissionService(IPermissionRepository permissionRepository, IUserRepository userRepository)
        {
            _permissionRepository = permissionRepository;
            _userRepository = userRepository;
        }

        #endregion

        public IEnumerable<Role> GetRoles() =>
            _permissionRepository.GetRoles();

        public async Task<AddOrEditRoleViewModel> GetRoleDataAsync(string roleId)
        {
            var role = await _permissionRepository.GetRoleByIdAsync(roleId);

            if (role is null) return null;

            var permissions = _permissionRepository.GetPermissions();
            var selectedPermissions = _permissionRepository.GetPermissionsIdOfRole(role.RoleId);

            return new AddOrEditRoleViewModel()
            {
                RoleId = role.RoleId,
                RoleTitle = role.RoleTitle,
                Permissions = permissions,
                SelectedPermissions = selectedPermissions
            };
        }

        public async Task<ResultTypes> AddRoleAsync(Role role)
        {
            try
            {
                await _permissionRepository.AddRoleAsync(role);
                await _permissionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditRoleAsync(Role role)
        {
            try
            {
                _permissionRepository.UpdateRole(role);
                await _permissionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<Role> GetRoleByIdAsync(string roleId)
            =>
                await _permissionRepository.GetRoleByIdAsync(roleId);

        public async Task<ResultTypes> DeleteRoleAsync(string roleId)
        {
            var role = await _permissionRepository.GetRoleByIdAsync(roleId);

            if (role == null)
                return ResultTypes.Failed;

            try
            {
                var userRoles = _permissionRepository.GetUserRolesByRoleId(role.RoleId);
                var rolePermissions = _permissionRepository.GetRolePermissionsOfRole(role.RoleId);

                if (userRoles != null)
                {
                    foreach (var userRole in userRoles)
                    {
                        _permissionRepository.RemoveUserRole(userRole);
                    }
                }

                if (rolePermissions != null)
                {
                    foreach (var rolePermission in rolePermissions)
                    {
                        _permissionRepository.RemoveRolePermission(rolePermission);
                    }
                }

                _permissionRepository.RemoveRole(role);
                await _permissionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<bool> IsRoleExistAsync(string roleId) =>
                await _permissionRepository.IsRoleExistAsync(roleId);

        public async Task<ResultTypes> AddUserToRoleAsync(string userId, params string[] rolesName)
        {

            List<Role> roles = new List<Role>();

            foreach (var item in rolesName)
            {
                var role = await _permissionRepository.GetRoleByNameAsync(item.FixedText());
                if (role is null)
                {
                    return ResultTypes.Failed;
                }
                roles.Add(role);
            }

            foreach (var item in roles)
            {
                var userRole = new UserRole(userId, item.RoleId);
                await _permissionRepository.AddUserRoleAsync(userRole);
            }

            await _userRepository.SaveChangesAsync();

            return ResultTypes.Successful;
        }

        public async Task<ResultTypes> AddUserToRoleAsync(string userId, List<string> rolesId)
        {
            try
            {
                if (!await _userRepository.IsUserExistAsync(userId))
                    return ResultTypes.Failed;

                foreach (var roleId in rolesId)
                {
                    if (await _permissionRepository.IsRoleExistAsync(roleId))
                    {
                        var userRole = new UserRole(userId, roleId);

                        await _permissionRepository.AddUserRoleAsync(userRole);
                    }
                }

                await _permissionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var role = await _permissionRepository.GetRoleByNameAsync(roleName);

            if (role is null) return ResultTypes.Failed;

            var userRole = await _permissionRepository.GetUserRoleAsync(userId, role.RoleId);
            if (userRole is null) return ResultTypes.Failed;


            try
            {
                _permissionRepository.RemoveUserRole(userRole);
                await _userRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> DeleteAllUserRolesByUserIdAsync(string userId)
        {
            if (!await _userRepository.IsUserExistAsync(userId)) return ResultTypes.Failed;


            var userRoles = _permissionRepository.GetUserRolesByUserId(userId);

            if (userRoles != null)
            {
                foreach (var userRole in userRoles)
                {
                    _permissionRepository.RemoveUserRole(userRole);
                }

                await _permissionRepository.SaveChangesAsync();
            }

            return ResultTypes.Successful;
        }

        public async Task<ResultTypes> AddUserRoleAsync(UserRole userRole)
        {
            try
            {
                await _permissionRepository.AddUserRoleAsync(userRole);
                await _permissionRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<Tuple<IEnumerable<Role>, int, int>> GetRolesOfUserWithPaginationAsync(string userId, int pageId = 1, int take = 15, string filter = null)
        {
            int skip = (pageId - 1) * take; // 1-1 * 4 = 0 , 2-1 * 4 = 4

            int count = await _permissionRepository.GetUserRolesCountAsync(userId, filter);

            var roles = _permissionRepository.GetRolesOfUserWithPagination(userId, skip, take, filter);

            int totalPages = (int)Math.Ceiling(1.0 * count / take);

            return new Tuple<IEnumerable<Role>, int, int>(roles, pageId, totalPages);
        }

        public async Task<Permission> GetPermissionByIdAsync(string permissionId) =>
            await _permissionRepository.GetPermissionByIdAsync(permissionId);

        public IEnumerable<Permission> GetPermissions() =>
            _permissionRepository.GetPermissions();

        public async Task<AddOrEditPermissionViewModel> GetPermissionDataAsync(string permissionId)
        {
            var permission = await _permissionRepository.GetPermissionByIdAsync(permissionId);

            if (permission == null)
                return null;

            var permissions = _permissionRepository.GetPermissions();
            var roles = _permissionRepository.GetRoles();
            var selectedRoles = _permissionRepository.GetRolesIdOfPermission(permission.PermissionId);

            return new AddOrEditPermissionViewModel()
            {
                PermissionId = permission.PermissionId,
                PermissionTitle = permission.PermissionTitle,
                ParentId = permission.ParentId,
                Permissions = permissions,
                Roles = roles,
                SelectedRoles = selectedRoles
            };
        }

        public async Task<ResultTypes> AddRolePermissionByRoleAsync(string roleId, IEnumerable<string> permissionsId)
        {
            try
            {
                foreach (var permissionId in permissionsId)
                {
                    var rolePermission = new RolePermission()
                    {
                        RoleId = roleId,
                        PermissionId = permissionId
                    };
                    await _permissionRepository.AddRolePermissionAsync(rolePermission);
                }

                await _permissionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddRolePermissionByPermissionAsync(string permissionId, List<string> rolesId)
        {
            try
            {
                foreach (var roleId in rolesId)
                {
                    var rolePermission = new RolePermission()
                    {
                        RoleId = roleId,
                        PermissionId = permissionId
                    };
                    await _permissionRepository.AddRolePermissionAsync(rolePermission);
                }

                await _permissionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> RemoveRolePermissionsByRoleId(string roleId)
        {
            var rolePermissions = _permissionRepository.GetRolePermissionsOfRole(roleId);
            if (rolePermissions == null)
                return ResultTypes.Failed;

            try
            {
                foreach (var rolePermission in rolePermissions)
                {
                    _permissionRepository.RemoveRolePermission(rolePermission);
                }

                await _permissionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> RemoveRolePermissionsByPermissionId(string permissionId)
        {
            var rolePermissions = _permissionRepository.GetRolePermissionsOfPermission(permissionId);
            if (rolePermissions == null)
                return ResultTypes.Failed;

            try
            {
                foreach (var rolePermission in rolePermissions)
                {
                    _permissionRepository.RemoveRolePermission(rolePermission);
                }

                await _permissionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<bool> IsPermissionExistsAsync(string permissionId) =>
            await _permissionRepository.IsPermissionExistsAsync(permissionId);

        public async Task<ResultTypes> DeletePermissionAsync(string permissionId)
        {
            try
            {
                var permission = await _permissionRepository.GetPermissionByIdAsync(permissionId);

                if (permission == null)
                    return ResultTypes.Failed;

                var rolePermissions = _permissionRepository.GetRolePermissionsOfPermission(permissionId);

                if (rolePermissions != null)
                {
                    foreach (var rolePermission in rolePermissions)
                    {
                        _permissionRepository.RemoveRolePermission(rolePermission);
                    }
                }

                _permissionRepository.RemovePermission(permission);

                await _permissionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> AddPermissionAsync(Permission permission)
        {
            try
            {
                await _permissionRepository.AddPermissionAsync(permission);

                await _permissionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> EditPermissionAsync(Permission permission)
        {
            try
            {
                _permissionRepository.UpdatePermission(permission);

                await _permissionRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<bool> PermissionCheckerAsync(string userId, params string[] permissions)
        {
            try
            {
                var userRoles = _permissionRepository.GetRolesIdOfUser(userId);

                if (userRoles == null)
                    return false;

                List<string> permissionsRolesId = new List<string>();


                foreach (var t in permissions)
                {
                    var permissionId = await _permissionRepository.GetPermissionIdByNameAsync(t);

                    if (string.IsNullOrEmpty(permissionId))
                        continue;

                    var roles = _permissionRepository.GetRolesIdOfPermission(permissionId);

                    if (roles == null)
                        continue;

                    permissionsRolesId.AddRange(roles);
                }
                return permissionsRolesId.Any(c => userRoles.Contains(c));
            }
            catch
            {
                return false;
            }
        }
    }
}