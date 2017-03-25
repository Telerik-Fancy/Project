﻿using System.Web;
using System.Web.Mvc;

namespace Fancy.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            filters.Add(new OutputCacheAttribute()
            {
                Duration = 3600
            });
        }
    }
}
