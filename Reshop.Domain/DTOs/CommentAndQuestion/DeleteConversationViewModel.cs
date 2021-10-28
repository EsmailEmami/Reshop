using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.CommentAndQuestion
{
    public class DeleteConversationViewModel
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "دلیل حذف")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string DeleteDescription { get; set; }
    }
}
