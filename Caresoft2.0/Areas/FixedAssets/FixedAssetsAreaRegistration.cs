using System.Web.Mvc;

namespace Caresoft2._0.Areas.FixedAssets
{
    public class FixedAssetsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FixedAssets";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FixedAssets_default",
                "FixedAssets/{controller}/{action}/{id}",
                new { controller= "MasterFixedAssets", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Caresoft2._0.Areas.FixedAssets.Controllers" }
            );
        }
    }
}