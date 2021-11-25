using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reshop.Domain.DTOs.Image
{
    public class ImageForShowViewModel
    {
        public string ImageId { get; set; }
        public string ImageName { get; set; }
        public string Url { get; set; }
        public string PlaceName { get; set; }
    }
}
