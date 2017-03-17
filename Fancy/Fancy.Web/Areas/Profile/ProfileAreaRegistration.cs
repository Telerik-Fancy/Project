using System.Web.Mvc;

namespace Fancy.Web.Areas.Profile
{
    public class ProfileAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Profile";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Profile_page",
                "Profile/{controller}/{action}",
                new { controller = "Profile", action = "ProfilePage" }
            );

            context.MapRoute(
                "Remove_item_from_profile",
                "Profile/{controller}/{action}/{itemId}",
                new { itemId = UrlParameter.Optional }
            );

            context.MapRoute(
                "Execute_order_from_profile",
                "Profile/{controller}/{action}/{orderId}",
                new { orderId = UrlParameter.Optional }
            );

            context.MapRoute(
                "Profile_default",
                "Profile/{controller}/{action}"
            );
        }
    }
}