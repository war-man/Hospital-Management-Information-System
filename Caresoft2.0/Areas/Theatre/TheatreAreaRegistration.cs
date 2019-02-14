using System.Web.Mvc;

namespace Caresoft2._0.Areas.Theatre
{
    public class TheatreAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Theatre";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Theatre_default",
                "Theatre/{controller}/{action}/{id}",
                 new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Caresoft2._0.Areas.Theatre.Controllers" }
            );
        }
    }
}