using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Reshop.Application.Convertors
{
    public class ImageConvertor
    {
        public void ImageResize(string inputImagePath, string outputImagePath, int newWidth)
        {
            const long quality = 50L;

            Bitmap sourceBitmap = new Bitmap(inputImagePath);

           

            double dblWidthOriginal = sourceBitmap.Width;

            double dblHeightOriginal = sourceBitmap.Height;

            double relationHeightWidth = dblHeightOriginal / dblWidthOriginal;

            int newHeight = (int)(newWidth * relationHeightWidth);



            //< create Empty Drawarea >

            var newDrawArea = new Bitmap(newWidth, newHeight);

            //</ create Empty Drawarea >



            using (var graphicsOfDrawArea = Graphics.FromImage(newDrawArea))

            {

                //< setup >

                graphicsOfDrawArea.CompositingQuality = CompositingQuality.HighSpeed;

                graphicsOfDrawArea.InterpolationMode = InterpolationMode.HighQualityBicubic;

                graphicsOfDrawArea.CompositingMode = CompositingMode.SourceCopy;

                //</ setup >



                //< draw into placeholder >

                //*imports the image into the drawarea

                graphicsOfDrawArea.DrawImage(sourceBitmap, 0, 0, newWidth, newHeight);

                //</ draw into placeholder >



                //--< Output as .Jpg >--

                using (var output = File.Open(outputImagePath, FileMode.Create))

                {

                    //< setup jpg >

                    var qualityParamId = Encoder.Quality;

                    var encoderParameters = new EncoderParameters(1);

                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);

                    //</ setup jpg >



                    //< save Bitmap as Jpg >

                    var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

                    newDrawArea.Save(output, codec, encoderParameters);

                    //resized_Bitmap.Dispose ();

                    output.Close();

                    //</ save Bitmap as Jpg >

                }

                //--</ Output as .Jpg >--

                graphicsOfDrawArea.Dispose();

            }

            sourceBitmap.Dispose();

            //---------------</ Image_resize() >---------------
        }
    }
}
