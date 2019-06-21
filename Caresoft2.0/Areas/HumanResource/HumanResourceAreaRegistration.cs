using System.Web.Mvc;

namespace Caresoft2._0.Areas.HumanResource
{
    public class HumanResourceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HumanResource";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HumanResource_default",
                "HumanResource/{controller}/{action}/{id}",
                new { Controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Caresoft2._0.Areas.HumanResource.Controllers" });
           
        }
    }
}