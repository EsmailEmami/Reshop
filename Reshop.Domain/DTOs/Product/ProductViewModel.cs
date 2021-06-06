namespace Reshop.Domain.DTOs.Product
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public decimal ProductPrice { get; set; }
        public string BrandName { get; set; }

        public string ShopperUserId { get; set; }
    }
}