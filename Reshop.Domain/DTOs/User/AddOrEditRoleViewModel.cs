using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.User
{
    public class AddOrEditRoleViewModel
    {
        public string RoleId { get; set; }

        [Display(Name = "مقام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string RoleTitle { get; set; }

        public IAsyncEnumerable<Entities.User.User> Users { get; set; }

        public IAsyncEnumerable<string> SelectedUsers { get; set; }
    }
}
