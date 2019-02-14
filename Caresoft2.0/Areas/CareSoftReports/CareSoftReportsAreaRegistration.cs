using System.Web.Mvc;

namespace Caresoft2._0.Areas.CareSoftReports
{
    public class CareSoftReportsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CareSoftReports";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CareSoftReports_default",
                "CareSoftReports/{controller}/{action}/{id}",
                 new { controller = "HomeReports", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Caresoft2._0.Areas.CareSoftReports.Controllers" }
            );
        }
    }
}