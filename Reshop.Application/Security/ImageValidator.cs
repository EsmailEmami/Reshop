using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Exception = System.Exception;

namespace Reshop.Application.Security
{
    public static class ImageValidator
    {
        public static bool IsImage(this IFormFile file)
        {
            try
            {
                 Image.FromStream(file.OpenReadStream());

                 return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
