using Microsoft.AspNetCore.Http;
using Reshop.Domain.Entities.Shopper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Reshop.Domain.Entities.Category;

namespace Reshop.Domain.DTOs.Product
{
    public class EditAuxViewModel
    {
        // product 
        public int ProductId { get; set; }

        [Display(Name = "نام کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(150, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string ProductTitle { get; set; }

        [Display(Name = "توضیحات کالا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public string Description { get; set; }

        [Display(Name = "نام اختصاصی برند")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public int OfficialBrandProductId { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        public bool IsActive { get; set; }

        public IEnumerable<StoreTitle> StoreTitles { get; set; }
        public IEnumerable<Tuple<int, string>> Brands { get; set; }
        public IEnumerable<Tuple<int, string>> OfficialProducts { get; set; }
        public IEnumerable<ChildCategory> ChildCategories { get; set; }

        public int SelectedStoreTitle { get; set; }
        public int SelectedBrand { get; set; }
        public int SelectedChildCategory { get; set; }

        // --------------------------------------------------------------------------- ITEMS

        [Display(Name = "نوع کابل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CableMaterial { get; set; }

        [Display(Name = "طول کابل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [Range(0, 99999, ErrorMessage = "{0} نمی تواند بیشتر از {2} کاراکتر باشد .")]
        public int CableLenght { get; set; }

        // ---------------------------------------------------------------------------IMG

        public List<IFormFile> Images { get; set; }


        // for show on edit
        public IEnumerable<string> SelectedImages { get; set; }

        public IEnumerable<string> ChangedImages { get; set; }

        public IEnumerable<string> DeletedImages { get; set; }
    }
}
