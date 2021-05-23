﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Reshop.Application.Enums;
using Reshop.Domain.DTOs.User;
using Reshop.Domain.Entities.User;

namespace Reshop.Application.Interfaces.User
{
    public interface IRoleService
    {
        IAsyncEnumerable<Role> GetRoles();
        Task<AddOrEditRoleViewModel> GetRoleDataAsync(string roleId);
        Task<ResultTypes> AddRoleAsync(Role role);
        Task<ResultTypes> EditRoleAsync(Role role);
        Task<Role> GetRoleByIdAsync(string roleId);
        Task<ResultTypes> RemoveRoleAsync(string roleId);
        Task<bool> IsRoleExistAsync(string roleId);

        Task<ResultTypes> AddUserToRoleAsync(string userId, string roleName);
        Task<ResultTypes> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<ResultTypes> RemoveAllUserRolesByUserIdAsync(string userId);

        Task<ResultTypes> AddUserRoleAsync(UserRole userRole);
    }
}