using System.Web.Mvc;

namespace Fancy.Web.Controllers
{
    public class HomeController : Controller
    {
        
        [OutputCache]
        public ActionResult HomePage()
        {
            return View();
        }
    }
}