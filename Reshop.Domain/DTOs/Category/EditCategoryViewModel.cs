using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Reshop.Domain.DTOs.Category;

public class EditCategoryViewModel
{
    public int CategoryId { get; set; }

    [Display(Name = "نام گروه")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
    [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
    public string CategoryTitle { get; set; }

    public bool IsActive { get; set; }


    // for show on edit
    public List<IFormFile> Images { get; set; }

    public IEnumerable<string> SelectedImages { get; set; }

    public IEnumerable<string> ChangedImages { get; set; }

    public IEnumerable<string> DeletedImages { get; set; }
    
    public IEnumerable<string> Urls { get; set; }

    public IEnumerable<string> ChangedUrls { get; set; }
}