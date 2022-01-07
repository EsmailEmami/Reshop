using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Linq;

namespace Reshop.Application.Security
{
    public static class FileValidator
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

        public static bool IsImage(this IFormFile file, params string[] ext)
        {
            try
            {
                Image.FromStream(file.OpenReadStream());

                return ext.Any(c => c == file.FileName);
            }
            catch
            {
                return false;
            }
        }
    }
}
