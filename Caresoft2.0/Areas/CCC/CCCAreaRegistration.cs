using System.Web.Mvc;

namespace Caresoft2._0.Areas.CCC
{
    public class CCCAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CCC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
               "CCC_default",
               "CCC/{controller}/{action}/{id}",
              new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "Caresoft2._0.Areas.CCC.Controllers" }

           );
        }
    }
}