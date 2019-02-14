using System.Web.Mvc;

namespace Caresoft2._0.Areas.PatientModule
{
    public class PatientModuleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PatientModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PatientModule_default",
                "PatientModule/{controller}/{action}/{id}",
                new { Controller = "Home", action = "Index", id = UrlParameter.Optional },
                                namespaces: new[] { "Caresoft2._0.Areas.PatientModule.Controllers" }
            );
        }
    }
}