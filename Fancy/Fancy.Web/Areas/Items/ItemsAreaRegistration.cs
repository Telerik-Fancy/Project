using System.Web.Mvc;

namespace Fancy.Web.Areas.Items
{
    public class ItemsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Items";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Items_default",
                "Items/{controller}/{action}",
                new { controller = "Items" },
                new[] { "Fancy.Web.Areas.Items.Controllers" }
            );
        }
    }
}