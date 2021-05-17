using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.User
{
    public class WalletType
    {
        public WalletType()
        {
            
        }

        // DatabaseGenerated(DatabaseGeneratedOption.None) => prevent to auto increment

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WalletTypeId { get; set; }

        [Required]
        [MaxLength(150)]
        public string TypeTitle { get; set; }

        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
