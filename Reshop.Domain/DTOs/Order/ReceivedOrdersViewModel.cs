using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Order
{
    public class ReceivedOrdersViewModel
    {
        public string OrderId { get; set; }

        public string TrackingCode { get; set; }

        public DateTime PayDate { get; set; }

        public decimal Sum{ get; set; }

        public IEnumerable<string> ProPics { get; set; }
    }
}
