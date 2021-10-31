using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Order
{
    public class OrderForShowInListViewModel
    {
        public string OrderId { get; set; }

        public string TrackingCode { get; set; }

        public DateTime PayDate { get; set; }

        public bool IsReceived { get; set; }

        public decimal Sum { get; set; }
    }
}
