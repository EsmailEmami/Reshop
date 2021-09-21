namespace Reshop.Domain.DTOs.Product
{
    public class AvailableBrandViewModel
    {
        public int BrandId { get; set; }
        public bool AvailableOfficialBrandProducts { get; set; }
        public bool AvailableProducts { get; set; }
    }
}