using Reshop.Domain.Entities.Product;

namespace Reshop.Domain.DTOs.Product
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public decimal ProductPrice { get; set; }

        public int ProductsCount { get; set; }

        public int BrandCount { get; set; }

        public string BrandName { get; set; }

        public int PageId { get; set; }

        public int TotalPages { get; set; }
    }
}