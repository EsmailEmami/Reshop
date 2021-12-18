using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.Permission
{
    public class RolePermission
    {
        public RolePermission()
        {
        }

        [ForeignKey("Role")]
        public string RoleId { get; set; }

        [ForeignKey("Permission")]
        public string PermissionId { get; set; }

        #region Relations

        public virtual Role Role { get; set; }

        public virtual Permission Permission { get; set; }

        #endregion
    }
}
