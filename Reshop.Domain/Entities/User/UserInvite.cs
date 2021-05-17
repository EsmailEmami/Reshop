using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.User
{
    public class UserInvite
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserInviteId { get; set; }

        [ForeignKey("User")]
        public string InviterUserId { get; set; }

        public string InvitedUserId { get; set; }

        #region Relations

        public virtual User User { get; set; }

        #endregion
    }
}
