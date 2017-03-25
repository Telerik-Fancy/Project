using System.Web;
using Microsoft.AspNet.Identity;
using Fancy.Web.WebUtils.Contracts;

namespace Fancy.Web.WebUtils
{
    public class IdentityProvider : IIdentityProvider
    {
        public string GetUserId()
        {
            return HttpContext.Current.User.Identity.GetUserId();
        }
    }
}
