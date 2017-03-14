using System.Drawing;
using System.Web;

namespace Fancy.Web.WebUtils.Contracts
{
    public interface IImageConverter
    {
        byte[] ConvertFileToByteArray(HttpPostedFileBase file);

        string ConvertByteArrayToImageString(byte[] array);
    }
}
