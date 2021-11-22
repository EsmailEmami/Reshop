using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.DTOs.User;

namespace Reshop.Domain.DTOs.Shopper
{
    public class ShopperDataForAdmin
    {
        public string ShopperId { get; set; }
        public string ShopperFullName { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalCode { get; set; }
        public DateTime BirthDay { get; set; }
        public string Email { get; set; }
        public string IssuanceOfIdentityCard { get; set; }
        public string FullIdOfIdentityCard { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<StoreAddressForShowViewModel> StoreAddresses { get; set; }
    }
}
