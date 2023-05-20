using Svg;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;


namespace API
{
    public static class SvgConverter
    {
        public static Image Svg2Png(String svg, int width, int height)
        {

            byte[] png_bytes;
            string png_base64;
            byte[] byte_array;
            Stream stream;
            SvgDocument svg_document;
            Bitmap bitmap;
            string base64_string;


            //convert svg string to byte array
            //NOTE : proper encoding needed else will shown funny values when render
            byte_array = Encoding.UTF8.GetBytes(svg);

            //convert byte array to stream
            stream = new MemoryStream(byte_array);

            Svg.SvgDocument.EnsureSystemIsGdiPlusCapable();


            //generate svg doc from stream
            svg_document = SvgDocument.Open<Svg.SvgDocument>(stream);

            //convert svg doc to bitmap with specified width & height
            bitmap = svg_document.Draw(width, height);

            return bitmap;

            //png_bytes = ImageToByte2(bitmap);

            //base64_string = Convert.ToBase64String(png_bytes, 0, png_bytes.Length);

            //png_base64 = "data:image/png;base64," + base64_string;

            //return png_bytes;

        }

        public static byte[] ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

    }
}
