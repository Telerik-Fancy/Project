using Fancy.Web.WebUtils.Contracts;
using Microsoft.AspNet.Identity;
using System.Web;

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
