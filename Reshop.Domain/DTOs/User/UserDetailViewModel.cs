using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.DTOs.User
{
    public class UserDetailViewModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string UserImage { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int OrdersCount { get; set; }
        public string NationalCode { get; set; }
        public int AddressesCount { get; set; }
        public DateTime RegisterDate { get; set; }

        // if shopper Id == null, the user is just user
        // else user is shopper too
        public string ShopperId { get; set; }

        public IEnumerable<AddressForShowViewModel> Addresses { get; set; }
    }
}
