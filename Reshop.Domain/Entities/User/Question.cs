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

        [Key]
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

        public bool IsDelete { get; set; } = false;

        [Display(Name = "دلیل حذف")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DeleteDescription { get; set; }

        #region Relations

        public virtual Product.Product Product { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual ICollection<QuestionLike> QuestionLikes { get; set; }
        public virtual ICollection<ReportQuestion> QuestionReports { get; set; }

        #endregion
    }
}
