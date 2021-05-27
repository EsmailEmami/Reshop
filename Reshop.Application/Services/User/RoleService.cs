using System;
using Reshop.Application.Enums;
using Reshop.Application.Interfaces.User;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;
using Reshop.Domain.Interfaces.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Convertors;

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

            var users = _userRepository.GetUsers();
            var selectedUsers = _roleRepository.GetUsersIdOfRole(roleId);

            return new AddOrEditRoleViewModel()
            {
                RoleId = role.RoleId,
                RoleTitle = role.RoleTitle,
                Users = users,
                SelectedUsers = selectedUsers
            };
        }

        public async Task<ResultTypes> AddRoleAsync(Role role)
        {
            try
            {
                var finalRole = new Role()
                {
                    RoleTitle = Fixer.FixedText(role.RoleTitle)
                };


                await _roleRepository.AddRoleAsync(finalRole);
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

                if (userRoles is not null)
                {
                    await foreach (var userRole in userRoles)
                    {
                        _roleRepository.RemoveUserRole(userRole);
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


        public async Task<ResultTypes> AddUserToRoleAsync(string userId, string roleName)
        {
            var role = await _roleRepository.GetRoleByNameAsync(Fixer.FixedText(roleName));

            if (role is null) return ResultTypes.Failed;

            var userRole = new UserRole(userId, role.RoleId);

            try
            {
                await _roleRepository.AddUserRoleAsync(userRole);
                await _userRepository.SaveChangesAsync();

                return ResultTypes.Successful;
            }
            catch
            {
                return ResultTypes.Failed;
            }
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

            if (userRoles is not  null)
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
    }
}