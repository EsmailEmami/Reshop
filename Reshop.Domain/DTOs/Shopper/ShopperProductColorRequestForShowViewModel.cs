using System;

namespace Reshop.Domain.DTOs.Shopper
{
    public class ShopperProductColorRequestForShowViewModel
    {
        public string ShopperProductColorRequestId { get; set; }
        public string ShopperId { get; set; }
        public string StoreName { get; set; }
        public string ShopperProductId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public bool RequestType { get; set; } // add or edit
        public DateTime RequestDate { get; set; }
        public bool IsRead { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string ColorName { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public string UserId { get; set; }
        public string UserFullName { get; set; }
    }
}