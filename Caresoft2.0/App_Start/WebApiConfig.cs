using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Caresoft2._0
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            /*TO return JSON formated data only*/
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            /*TO ident JSON formated data neatly*/
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        }
    }
}
