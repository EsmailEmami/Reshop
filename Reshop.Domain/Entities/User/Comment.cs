using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.User
{
    public class Comment
    {
        public Comment()
        {
            
        }

        [Key]
        public int CommentId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Display(Name = "عنوان متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CommentTitle { get; set; }

        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(4000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CommentText { get; set; }

        [Display(Name = "تاریخ ثبت نظر")]
        public DateTime CommentDate { get; set; }

        #region Relations

        public virtual Product.Product Product { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<CommentAnswer> CommentAnswers { get; set; }

        #endregion
    }
}
