using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Shopper
{
    public  class ShoppersGeneralDataForAdmin
    {
        public ShoppersGeneralDataForAdmin(int shoppersCount, int activeShoppersCount, int existedShoppersCount)
        {
            ShoppersCount = shoppersCount;
            ActiveShoppersCount = activeShoppersCount;
            ExistedShoppersCount = existedShoppersCount;
        }

        public int ShoppersCount { get; set; }
        public int ActiveShoppersCount { get; set; }
        public int ExistedShoppersCount { get; set; }
    }
}
