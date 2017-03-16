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
                "Items_single",
                "Items/{controller}/{action}/{itemId}",
                new { controller = "Items", action = "SingleItem", itemId = UrlParameter.Optional },
                new[] { "Fancy.Web.Areas.Items.Controllers" }
            );

            context.MapRoute(
                "Items_gallery",
                "Items/{controller}/{action}/{pageNumber}/{itemType}",
                new[] { "Fancy.Web.Areas.Items.Controllers" }
            );
        }
    }
}