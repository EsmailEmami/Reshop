using Reshop.Domain.Entities.Permission;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.Permission
{
    public class AddOrEditPermissionViewModel
    {
        public int PermissionId { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string PermissionTitle { get; set; }

        public int ParentId { get; set; }

        public IEnumerable<Entities.Permission.Permission> Permissions { get; set; }

        // role
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<string> SelectedRoles { get; set; }
    }
}
