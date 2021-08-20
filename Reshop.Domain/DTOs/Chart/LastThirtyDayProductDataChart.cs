using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Chart
{
    public class LastThirtyDayProductDataChart
    {
        public string Date { get; set; }
        public int ViewCount { get; set; }
        public int SellCount { get; set; }
    }
}
