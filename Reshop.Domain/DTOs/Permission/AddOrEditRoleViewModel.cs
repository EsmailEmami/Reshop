using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.Permission
{
    public class AddOrEditRoleViewModel
    {
        public string RoleId { get; set; }

        [Display(Name = "مقام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RoleTitle { get; set; }

        public IEnumerable<Entities.Permission.Permission> Permissions { get; set; }

        public IEnumerable<string> SelectedPermissions { get; set; }
    }
}