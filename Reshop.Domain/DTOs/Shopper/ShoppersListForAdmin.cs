using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Shopper
{
    public class ShoppersListForAdmin
    {
        public string ShopperUserId { get; set; }
        public string ShopperName { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreName { get; set; }
    }
}
