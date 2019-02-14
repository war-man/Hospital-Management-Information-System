using System.Web.Mvc;

namespace Caresoft2._0.Areas.BloodBank
{
    public class BloodBankAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "BloodBank";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "BloodBank_default",
                "BloodBank/{controller}/{action}/{id}",
                new { Controller = "Home", action = "Index", id = UrlParameter.Optional },
                                                namespaces: new[] { "Caresoft2._0.Areas.BloodBank.Controllers" });
        }
    }
}