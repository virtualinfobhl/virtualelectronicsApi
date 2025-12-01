using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CityStoreAPIWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{Exid}/{Exxid}/{Exxxid}/{Exxxxid}/{pera6}/{pera7}/{pera8}/{pera9}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, Exid = UrlParameter.Optional, Exxid = UrlParameter.Optional, Exxxid = UrlParameter.Optional, Exxxxid = UrlParameter.Optional, pera6 = UrlParameter.Optional, pera7 = UrlParameter.Optional, pera8 = UrlParameter.Optional, pera9 = UrlParameter.Optional }
            );
            
        }
    }
}