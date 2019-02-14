using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Caresoft2._0.Controllers
{
    public class TriValueController : ApiController
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        [System.Web.Http.HttpPost]
        public HttpResponseMessage SendNotification(Notification obj)
        {
            NotificationHub objNotifHub = new NotificationHub();
            Notification objNotif = new Notification();
          //  objNotif.SentTo = obj.UserID;

           // context.Configuration.ProxyCreationEnabled = false;
          //  context.Notifications.Add(objNotif);
          //  context.SaveChanges();

         //   objNotifHub.SendNotification(objNotif.SentTo);

            //var query = (from t in context.Notifications  
            //             select t).ToList();  

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}