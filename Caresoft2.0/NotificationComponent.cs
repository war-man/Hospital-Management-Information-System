using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using CaresoftHMISDataAccess;

namespace Caresoft2._0
{
    public class NotificationComponent
    {

        //Here we will add a function for register notification (will add sql dependency)
        public void RegisterNotification(DateTime currentTime)
        {
            string conStr = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
            string sqlCommand = @"SELECT * from [dbo].[OpdRegisters] where [TimeAdded] > @TimeAdded";
            string sql2 = @"SELECT * from [dbo].[OpdRegisters] where [TimeAdded] > @TimeAdded";
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
                SqlCommand cmd2 = new SqlCommand(sql2, con);
                cmd.Parameters.AddWithValue("@TimeAdded", currentTime);
                cmd2.Parameters.AddWithValue("@TimeAdded", currentTime);
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                cmd2.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                SqlDependency sqlDep2 = new SqlDependency(cmd2);
                sqlDep.OnChange += sqlDep_OnChange;
                sqlDep2.OnChange += sqlDep_OnChange;
                //we must have to execute the command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // nothing need to add here now
                }
                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    // nothing need to add here now
                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                SqlDependency sqlDep2 = sender as SqlDependency;
                //sqlDep.OnChange -= sqlDep_OnChange;
                sqlDep2.OnChange += sqlDep_OnChange;


                

                //from here we will send notification message to client
                
                

                //re-register notification
                RegisterNotification(DateTime.Now);
            }



        }



        
        public List<OpdRegister> GetContacts(DateTime afterDate)
                {
                    using (CaresoftHMISEntities dc = new CaresoftHMISEntities())
                    {
                        return dc.OpdRegisters.Where(a => a.TimeAdded > afterDate).OrderByDescending(a => a.TimeAdded).ToList();
                    }
                }
                


    }
}