using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Reshop.Domain.DTOs.Product
{
    public class AddOrEditProductImageViewModel
    {
        public string ProductGalleryId { get; set; }
        public IFormFile Image { get; set; }
    }
}
