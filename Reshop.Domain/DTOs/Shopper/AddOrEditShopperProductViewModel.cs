using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Entities.Shopper;

namespace Reshop.Domain.DTOs.Shopper
{
    public class AddOrEditShopperProductViewModel
    {
        public string ShopperId { get; set; }

        public IEnumerable<Tuple<int, string>> StoreTitles { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "گارانتی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Warranty { get; set; }

        public bool IsActive { get; set; }
    }
}
