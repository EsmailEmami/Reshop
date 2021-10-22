using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reshop.Domain.Entities.User
{
    public class ReportComment
    {
        public ReportComment()
        {
        }

        [ForeignKey("Comment")]
        public int CommentId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("ReportCommentType")]
        public int ReportCommentTypeId { get; set; }


        [Display(Name = "توضیحات")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        #region Relations

        public virtual ReportCommentType ReportCommentType { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual User User { get; set; }

        #endregion
    }
}
