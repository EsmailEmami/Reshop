using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Shopper
{
    public class AddOrEditShopperProductDiscountViewModel
    {

        public string ShopperProductColorId { get; set; }

        public byte DiscountPercent { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
    }
}
