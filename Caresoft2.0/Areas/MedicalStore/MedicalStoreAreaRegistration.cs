using System.Web.Mvc;

namespace Caresoft2._0.Areas.MedicalStore
{
    public class MedicalStoreAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MedicalStore";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MedicalStore_default",
                "MedicalStore/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Caresoft2._0.Areas.MedicalStore.Controllers" }
            );
        }
    }
}