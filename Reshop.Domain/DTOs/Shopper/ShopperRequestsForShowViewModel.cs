using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Shopper
{
    public class ShopperRequestsForShowViewModel
    {
        public string RequestId { get; set; }
        public string RequestType { get; set; } // color or new product
        public bool Type { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsSuccess { get; set; }
        public string Description { get; set; }
    }
}
