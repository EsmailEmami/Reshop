using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.Entities.User
{
    public class CommentAnswer
    {
        public CommentAnswer()
        {
            
        }

        [Key]
        public int CommentAnswerId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Comment")]
        public int CommentId { get; set; }

        [Display(Name = "متن جواب")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(4000, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string AnswerText { get; set; }

        [Display(Name = "تاریخ ثبت نظر")]
        public DateTime CommentDate { get; set; }

        #region Relations

        public virtual Comment Comment { get; set; }

        public virtual User User { get; set; }

        #endregion
    }
}
