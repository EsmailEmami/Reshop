using Microsoft.AspNetCore.Http;
using Reshop.Domain.Entities.Image;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.DTOs.Image
{
    public class AddOrEditImageViewModel
    {
        public string ImageId { get; set; }

        public IFormFile Image { get; set; }

        [Display(Name = "ادرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ImageUrl { get; set; }

        public int SelectedPlace { get; set; }

        public string SelectedImage { get; set; }

        public IEnumerable<ImagePlace> Places { get; set; }
    }
}
