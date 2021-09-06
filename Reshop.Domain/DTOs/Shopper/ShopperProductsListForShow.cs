using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Shopper
{
    public class ShopperProductsListForShow
    {
        public string ShopperProductId { get; set; }
        public string ProductName { get; set; }
        public bool IsActive { get; set; }
        public int ColorsCount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
