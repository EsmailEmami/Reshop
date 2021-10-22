using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reshop.Domain.Entities.User;

namespace Reshop.Domain.DTOs.CommentAndQuestion
{
    public class AddReportCommentViewModel
    {
        [Required]
        public int CommentId { get; set; }

        public int SelectedType { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Description { get; set; }


        public IEnumerable<ReportCommentType> Types { get; set; }
    }
}
