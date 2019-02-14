using System.Web.Mvc;

namespace Caresoft2._0.Areas.Nutrition
{
    public class NutritionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Nutrition";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Nutrition_default",
                "Nutrition/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                                namespaces: new[] { "Caresoft2._0.Areas.Nutrition.Controllers" }

            );
        }
    }
}