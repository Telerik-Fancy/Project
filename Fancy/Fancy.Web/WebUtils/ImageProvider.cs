using System;
using System.IO;
using System.Web;
using Fancy.Web.WebUtils.Contracts;

namespace Fancy.Web.WebUtils
{
    public class ImageProvider : IImageProvider
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
            string base64 = Convert.ToBase64String(array);

            return string.Format("data:image/gif;base64,{0}", base64);
        }
    }
}