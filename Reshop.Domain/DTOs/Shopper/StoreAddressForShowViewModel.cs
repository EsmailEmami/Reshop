using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Shopper
{
    public class StoreAddressForShowViewModel
    {
        public string StoreAddressId { get; set; }
        public string StoreName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Plaque { get; set; }
        public string PostalCode { get; set; }
        public string AddressText { get; set; }
        public string LandlinePhoneNumber { get; set; }
    }
}
