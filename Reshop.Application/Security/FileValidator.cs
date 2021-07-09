using Microsoft.AspNetCore.Http;
using System.Drawing;

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
    }
}
