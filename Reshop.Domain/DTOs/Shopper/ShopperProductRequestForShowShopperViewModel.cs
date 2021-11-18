using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Shopper
{
    public class ShopperProductRequestForShowShopperViewModel
    {
        public string ShopperProductRequestId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public bool RequestType { get; set; } // add or edit
        public DateTime RequestDate { get; set; }
        public string Warranty { get; set; }
        public bool IsRead { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string UserFullName { get; set; }
    }
}
