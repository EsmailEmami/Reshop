using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Permission
{
    public class Role
    {
        public Role()
        {

        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RoleId { get; set; }

        [Display(Name = "مقام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RoleTitle { get; set; }


        #region Relations

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        #endregion
    }
}
