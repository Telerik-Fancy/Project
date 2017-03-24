using System.Web.Mvc;

namespace Fancy.Web.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult HomePage()
        {
            return View();
        }
    }
}