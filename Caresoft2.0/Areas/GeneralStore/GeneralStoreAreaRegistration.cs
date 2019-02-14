using System.Web.Mvc;

namespace Caresoft2._0.Areas.GeneralStore
{
    public class GeneralStoreAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GeneralStore";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GeneralStore_default",
                "GeneralStore/{controller}/{action}/{id}",
                new { controller= "MasterGStore", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Caresoft2._0.Areas.GeneralStore.Controllers" }
            );
        }
    }
}