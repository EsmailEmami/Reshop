using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.User
{
    public class Question
    {
        public Question()
        {
            
        }

        public int QuestionId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Display(Name = "عنوان سوال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string QuestionTitle { get; set; }

        [Display(Name = "متن سوال")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(4000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string QuestionText { get; set; }

        [Display(Name = "تاریخ ثبت نظر")]
        public DateTime QuestionDate { get; set; }

        public bool IsRead { get; set; }

        #region Relations

        public virtual Product.Product Product { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }

        #endregion
    }
}
