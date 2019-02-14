using System.Web.Mvc;

namespace Caresoft2._0.Areas.MISReports
{
    public class MISReportsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MISReports";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MISReports_default",
                "MISReports/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                                namespaces: new[] { "Caresoft2._0.Areas.MISReports.Controllers" }
            );
        }
    }
}