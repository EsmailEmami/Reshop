using Reshop.Application.Convertors;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.Permission;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Application.Services.User
{
    public class RoleService : IRoleService
    {
        #region constructor

        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public RoleService(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        #endregion

        public IAsyncEnumerable<Role> GetRoles() => _roleRepository.GetRoles();

        public async Task<AddOrEditRoleViewModel> GetRoleDataAsync(string roleId)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);

            if (role is null) return null;

            var permissions = _roleRepository.GetPermissions();
            var selectedPermissions = _roleRepository.GetPermissionsIdOfRole(role.RoleId);

            return new AddOrEditRoleViewModel()
            {
                RoleId = role.RoleId,
                RoleTitle = role.RoleTitle,
                Permissions = permissions as IEnumerable<Permission>,
                SelectedPermissions = selectedPermissions as IEnumerable<int>
            };
        }

        public async Task<ResultTypes> AddRoleAsync(Role role)
        {
            try
            {
                await _roleRepository.AddRoleAsync(role);
                await _roleRepository.SaveChangesAsync();

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
                _roleRepository.UpdateRole(role);
                await _roleRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<Role> GetRoleByIdAsync(string roleId)
            =>
                await _roleRepository.GetRoleByIdAsync(roleId);

        public async Task<ResultTypes> RemoveRoleAsync(string roleId)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);

            if (role is null) return ResultTypes.Failed;

            try
            {
                var userRoles = _roleRepository.GetUserRolesByRoleId(role.RoleId);
                var rolePermissions = _roleRepository.GetRolePermissionsOfRole(role.RoleId);

                if (userRoles is not null)
                {
                    await foreach (var userRole in userRoles)
                    {
                        _roleRepository.RemoveUserRole(userRole);
                    }

                    await foreach (var rolePermission in rolePermissions)
                    {
                        _roleRepository.RemoveRolePermission(rolePermission);
                    }

                    await _roleRepository.SaveChangesAsync();
                }

                _roleRepository.RemoveRole(role);
                await _roleRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<bool> IsRoleExistAsync(string roleId)
            =>
                await _roleRepository.IsRoleExistAsync(roleId);


        public async Task<ResultTypes> AddUserToRoleAsync(string userId, string rolesName)
        {
            string[] roleName = rolesName.Split(",");

            List<Role> roles = new List<Role>();

            foreach (var item in roleName)
            {
                var role = await _roleRepository.GetRoleByNameAsync(item.FixedText());
                if (role is null)
                {
                    return ResultTypes.Failed;
                }
                roles.Add(role);
            }

            foreach (var item in roles)
            {
                var userRole = new UserRole(userId, item.RoleId);
                await _roleRepository.AddUserRoleAsync(userRole);
            }

            await _userRepository.SaveChangesAsync();

            return ResultTypes.Successful;
        }

        public async Task<ResultTypes> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var role = await _roleRepository.GetRoleByNameAsync(roleName);

            if (role is null) return ResultTypes.Failed;

            var userRole = await _roleRepository.GetUserRoleAsync(userId, role.RoleId);
            if (userRole is null) return ResultTypes.Failed;


            try
            {
                _roleRepository.RemoveUserRole(userRole);
                await _userRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> RemoveAllUserRolesByUserIdAsync(string userId)
        {
            if (!await _userRepository.IsUserExistAsync(userId)) return ResultTypes.Failed;


            var userRoles = _roleRepository.GetUserRolesByUserId(userId);

            if (userRoles is not null)
            {
                await foreach (var userRole in userRoles)
                {
                    _roleRepository.RemoveUserRole(userRole);
                }

                await _roleRepository.SaveChangesAsync();
            }

            return ResultTypes.Successful;
        }

        public async Task<ResultTypes> AddUserRoleAsync(UserRole userRole)
        {
            try
            {
                await _roleRepository.AddUserRoleAsync(userRole);
                await _roleRepository.SaveChangesAsync();
                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }


        public IAsyncEnumerable<Permission> GetPermissions()
            =>
                _roleRepository.GetPermissions();

        public async Task<ResultTypes> AddPermissionsToRoleAsync(string roleId, List<int> permissionsId)
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
                    await _roleRepository.AddRolePermissionAsync(rolePermission);
                }

                await _roleRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public async Task<ResultTypes> RemoveRolePermissionsByRoleId(string roleId)
        {
            var rolePermissions = _roleRepository.GetRolePermissionsOfRole(roleId);
            if (rolePermissions is null) return ResultTypes.Failed;

            try
            {
                await foreach (var rolePermission in rolePermissions)
                {
                    _roleRepository.RemoveRolePermission(rolePermission);
                }

                await _roleRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
        }

        public IAsyncEnumerable<string> GetPermissionRolesIdByPermission(string permissionName)
        {
            int permissionId = _roleRepository.GetPermissionIdByName(permissionName);

            return _roleRepository.GetRolesIdOfPermission(permissionId);
        }

        public bool PermissionChecker(string userId, string permissions)
        {
            string[] permission = permissions.Split(",");


            var userRoles = _roleRepository.GetRolesIdOfUser(userId) as IEnumerable<string>;
            if (userRoles is null) return false;


            List<string> permissionsRolesId = new List<string>();


            foreach (var t in permission)
            {
                var permissionId = _roleRepository.GetPermissionIdByName(t);

                if (permissionId != 0)
                {
                    var roles = (IEnumerable<string>)_roleRepository.GetRolesIdOfPermission(permissionId);

                    foreach (var role in roles)
                    {
                        permissionsRolesId.Add(role);
                    }
                }
            }

            return permissionsRolesId.Any(c => userRoles.Contains(c));

        }
    }
}