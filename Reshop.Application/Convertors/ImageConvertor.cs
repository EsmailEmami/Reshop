using Microsoft.AspNetCore.Http;
using Reshop.Application.Generator;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Reshop.Application.Convertors
{
    public static class ImageConvertor
    {
        public static void ImageResize(string inputImagePath, string outputImagePath, int newWidth)
        {
            const long quality = 100L;

            Bitmap sourceBitmap = new Bitmap(inputImagePath);


            double dblWidthOriginal = sourceBitmap.Width;

            double dblHeightOriginal = sourceBitmap.Height;

            double relationHeightWidth = dblHeightOriginal / dblWidthOriginal;

            int newHeight = (int)(newWidth * relationHeightWidth);

            var newDrawArea = new Bitmap(newWidth, newHeight);

            using (var graphicsOfDrawArea = Graphics.FromImage(newDrawArea))
            {
                graphicsOfDrawArea.CompositingQuality = CompositingQuality.HighQuality;

                graphicsOfDrawArea.InterpolationMode = InterpolationMode.HighQualityBicubic;

                graphicsOfDrawArea.CompositingMode = CompositingMode.SourceCopy;

                graphicsOfDrawArea.DrawImage(sourceBitmap, 0, 0, newWidth, newHeight);

                using (var output = File.Open(outputImagePath, FileMode.Create))
                {
                    var qualityParamId = Encoder.Quality;

                    var encoderParameters = new EncoderParameters(1)
                    {
                        Param = { [0] = new EncoderParameter(qualityParamId, quality) }
                    };

                    var codec = ImageCodecInfo.GetImageDecoders()
                        .FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

                    newDrawArea.Save(output, codec, encoderParameters);

                    output.Close();
                }

                graphicsOfDrawArea.Dispose();
            }

            sourceBitmap.Dispose();
        }

        public static async Task<string> CreateNewImage(IFormFile image, string path, int thumbWidth = 900)
        {
            string originalImgName = NameGenerator.GenerateUniqCodeWithDash() + Path.GetExtension(image.FileName);
            string thumbImgName = NameGenerator.GenerateUniqCodeWithDash() + Path.GetExtension(image.FileName);

            string originalFilePath = path + "/" + originalImgName;
            string thumbFilePath = path + "/" + thumbImgName;

            // create original image
            await using (var stream = new FileStream(originalFilePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }


            // ---------------thumb---------------

            // create thumb image
            ImageResize(originalFilePath, thumbFilePath, thumbWidth);

            // delete original Image
            DeleteImage(originalFilePath);

            return thumbImgName;
        }

        public static void DeleteImage(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
