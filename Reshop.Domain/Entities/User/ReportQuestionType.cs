using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.User
{
    public class ReportQuestionType
    {
        [Key]
        public int ReportQuestionTypeId { get; set; }

        [Display(Name = "عنوان ریپورت کامنت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ReportQuestionTitle { get; set; }
    }
}
