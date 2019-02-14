using System.Web.Mvc;

namespace Caresoft2._0.Areas.PharmacyModule
{
    public class PharmacyModuleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PharmacyModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PharmacyModule_default",
                "PharmacyModule/{controller}/{action}/{id}",
                new {controller= "PharmacyMaster", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Caresoft2._0.Areas.PharmacyModule.Controllers" }
            );
        }
    }
}