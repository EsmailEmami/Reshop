using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.Permission
{
    public class Permission
    {
        public Permission()
        {
        }

        [Key]
        public int PermissionId { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string PermissionTitle { get; set; }


        /// <summary>
        /// this prop is a relation in this table

        ///ParentId ManageUsers    Null

        ///Id AddUser ParentId  ---- here the parent of this row is first row

        /// If ParentId is NULL means its parent row
        /// </summary>


        public int? ParentId { get; set; }


        #region Relations

        [ForeignKey("ParentId")]
        public  virtual ICollection<Permission> Permissions { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        #endregion
    }
}
