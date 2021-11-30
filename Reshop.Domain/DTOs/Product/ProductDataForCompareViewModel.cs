using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductDataForCompareViewModel
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public object Detail { get; set; }
        public string ImageName { get; set; }
    }
}
