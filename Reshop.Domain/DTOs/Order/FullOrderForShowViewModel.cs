using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Order
{
    public class FullOrderForShowViewModel
    {
        public string TrackingCode { get; set; }
        public DateTime PayDate { get; set; }
        public decimal ShoppingCost { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal Sum { get; set; }
        public bool IsPayed { get; set; }
        public bool IsReceived { get; set; }

        public IEnumerable<OrderDetailForShowViewModel> Details { get; set; }
    }
}
