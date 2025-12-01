using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CityStoreAPIWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}/{Exid}/{Exxid}/{Exxxid}/{Exxxxid}/{pera6}/{pera7}/{pera8}/{pera9}",
                defaults: new { id = RouteParameter.Optional, Exid = RouteParameter.Optional, Exxid = RouteParameter.Optional, Exxxid = RouteParameter.Optional, Exxxxid = RouteParameter.Optional, pera6 = RouteParameter.Optional, pera7 = RouteParameter.Optional, pera8 = RouteParameter.Optional, pera9 = RouteParameter.Optional }
                
        );
            

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
        }
    }
}
