using System.Web.Mvc;

namespace Caresoft2._0.Areas.Radiology
{
    public class RadiologyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Radiology";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Radiology_default",
                "Radiology/{controller}/{action}/{id}",
                new { controller = "Setup", action ="Index", id = UrlParameter.Optional },
                                                namespaces: new[] { "Caresoft2._0.Areas.Radiology.Controllers" }


            );
        }
    }
}