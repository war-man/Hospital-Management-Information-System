using System.Web.Mvc;

namespace Caresoft2._0.Areas.Pathology
{
    public class PathologyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Pathology";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Pathology_default",
                "Pathology/{controller}/{action}/{id}",
                new { controller = "SetUp", action = "Index", id = UrlParameter.Optional },
                                namespaces: new[] { "Caresoft2._0.Areas.Pathology.Controllers" }

            );
        }
    }
}