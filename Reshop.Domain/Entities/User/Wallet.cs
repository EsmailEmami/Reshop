using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.User
{
    public class Wallet
    {
        public Wallet()
        {

        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string WalletId { get; set; }

        [Display(Name = "نوع تراکنش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [ForeignKey("WalletType")]
        public int WalletTypeId { get; set; }

        [Display(Name = "کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Range(10000, 3000000, ErrorMessage = "{0} نمی تواند کمتر از {1} تومان و بیشتر از {2} تومان باشد.")]
        public decimal Amount { get; set; }

        [Display(Name = "شرح")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Description { get; set; }

        [Display(Name = "تایید شده")]
        [Required]
        public bool IsPayed { get; set; }

        [Display(Name = "تاریخ و ساعت")]
        [Required]
        public DateTime CreateDate { get; set; }


        public virtual User User { get; set; }
        public virtual WalletType WalletType { get; set; }

    }
}
