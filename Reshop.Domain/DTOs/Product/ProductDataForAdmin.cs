using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductDataForAdmin
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string BrandName { get; set; }
        public string OfficialName { get; set; }
    }
}
