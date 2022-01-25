using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.Category;

public class AddCategoryViewModel
{
    [Display(Name = "نام گروه")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string CategoryTitle { get; set; }

    public bool IsActive { get; set; }

    // images
    [Display(Name = "تصاویر کالا")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    public List<IFormFile> Images { get; set; }


    [Display(Name = "آدرس های تصاویر")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    public List<string> Urls { get; set; }
}
