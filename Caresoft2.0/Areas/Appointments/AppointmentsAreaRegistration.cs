using System.Web.Mvc;

namespace Caresoft2._0.Areas.Appointments
{
    public class AppointmentsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Appointments";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Appointments_default",
                "Appointments/{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Caresoft2._0.Areas.Appointments.Controllers" }
            );
        }
    }
}