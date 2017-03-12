﻿using System.Web.Mvc;

namespace Fancy.Web.Areas.Admin
{
    public class AdminRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}"
            );
        }
    }
}