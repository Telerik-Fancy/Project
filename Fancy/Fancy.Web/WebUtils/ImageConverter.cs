using Fancy.Web.WebUtils.Contracts;
using System;
using System.Drawing;
using System.IO;
using System.Web;

namespace Fancy.Web.WebUtils
{
    public class ImageConverter : IImageConverter
    {
        public object BitmapFactory { get; private set; }

        public byte[] ConvertFileToByteArray(HttpPostedFileBase file)
        {
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            return target.ToArray();
        }

        public string ConvertByteArrayToImageString(byte[] array)
        {
            return Convert.ToBase64String(array);
        }
    }
}