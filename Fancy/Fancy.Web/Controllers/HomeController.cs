using System.Web.Mvc;

namespace Fancy.Web.Controllers
{
    public class HomeController : Controller
    {
        //[OutputCache(Duration = 60)]
        public ActionResult HomePage()
        {
            return View();
        }
    }
}