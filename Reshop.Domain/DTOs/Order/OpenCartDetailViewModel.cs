using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Order
{
    public class OpenCartDetailViewModel
    {
        public int ProductsCount { get; set; }
        public decimal ProductPrice { get; set; }
        public Tuple<byte, DateTime> Discount { get; set; }
    }
}
