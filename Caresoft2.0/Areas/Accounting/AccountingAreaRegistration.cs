using System.Web.Mvc;

namespace Caresoft2._0.Areas.Accounting
{
    public class AccountingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Accounting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Accounting_default",
                "Accounting/{controller}/{action}/{id}",
                new { Controller = "Home", action = "EnterBills", id = UrlParameter.Optional },
                                namespaces: new[] { "Caresoft2._0.Areas.Accounting.Controllers" }
            );
        }
    }
}