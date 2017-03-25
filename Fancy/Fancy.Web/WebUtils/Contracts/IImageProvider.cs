using System.Web;

namespace Fancy.Web.WebUtils.Contracts
{
    public interface IImageProvider
    {
        byte[] ConvertFileToByteArray(HttpPostedFileBase file);

        string ConvertByteArrayToImageString(byte[] array);
    }
}
