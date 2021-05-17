using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.Shopper
{
    public class ShopperStoreTitle
    {
        public ShopperStoreTitle()
        {

        }

        [ForeignKey("Shopper")]
        public string ShopperId { get; set; }

        [ForeignKey("StoreTitle")]
        public int StoreTitleId { get; set; }

        #region Relations

        public virtual Shopper Shopper { get; set; }

        public virtual StoreTitle StoreTitle { get; set; }

        #endregion
    }
}
