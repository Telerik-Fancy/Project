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
                "Add_Promotion",
                "Items/{controller}/{action}/{itemId}/{discount}",
                new { controller = "Promotions", action = "AddPromotion", itemId = UrlParameter.Optional, discount = UrlParameter.Optional }
            );

            context.MapRoute(
                "Remove_promotion",
                "Items/{controller}/{action}/{itemId}/",
                new { controller = "Promotions", action = "RemovePromotion", itemId = UrlParameter.Optional }
            );

            context.MapRoute(
                "Items_single",
                "Items/{controller}/{action}/{itemId}/",
                new { controller = "Items", action = "SingleItem", itemId = UrlParameter.Optional }
            );

            context.MapRoute(
                "Items_gallery",
                "Items/{controller}/{action}/{pageNumber}/{itemType}",
                new { itemType = UrlParameter.Optional }
            );
        }
    }
}