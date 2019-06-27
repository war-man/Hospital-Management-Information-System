using System.Web.Mvc;

namespace Caresoft2._0.Areas.Payroll
{
    public class PayrollAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Payroll";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Payroll_default",
                "Payroll/{controller}/{action}/{id}",
                new { Controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "Caresoft2._0.Areas.Payroll.Controllers" }
            );
        }
    }
}