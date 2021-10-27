using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.CommentAndQuestion
{
    public class AddReportConversationViewModel
    {
        [Required]
        public int Id { get; set; }

        public int SelectedType { get; set; }

        [Display(Name = "توضیحات")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string Description { get; set; }


        public IEnumerable<Tuple<int, string>> Types { get; set; }
    }
}
