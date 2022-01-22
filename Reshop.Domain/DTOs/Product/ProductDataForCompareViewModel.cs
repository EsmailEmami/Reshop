using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Product.ProductDetail;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductDataForCompareViewModel
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string ImageName { get; set; }

        public AUXDetail AuxDetail { get; set; }
        public MobileDetail MobileDetail { get; set; }
        public LaptopDetail LaptopDetail { get; set; }
    }
}
