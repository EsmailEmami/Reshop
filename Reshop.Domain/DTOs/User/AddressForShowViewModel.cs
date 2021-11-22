using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.User
{
    public class AddressForShowViewModel
    {
        public string AddressId { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string CityName { get; set; }
        public string StateName { get; set; }

        public string Plaque { get; set; }

        public string PostalCode { get; set; }

        public string AddressText { get; set; }
    }
}
