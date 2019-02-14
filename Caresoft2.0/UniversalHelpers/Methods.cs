using Caresoft2._0.Models;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.UniversalHelpers
{
    public class Methods
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        public int InsertTATLogs(TATLog tATLogs)
        {
            db.TATLogs.Add(tATLogs);

            return db.SaveChanges();
        }

        public int QueueTime(int? id, string Department)
        {
            if (id != null)
            {
                var opd = db.OpdRegisters.FirstOrDefault(e => e.Id == (id));
                opd.QueueTime = DateTime.Now;
                opd.FromDept = Department;
                // opdregister.QueueTime = DateTime.Now;


                return db.SaveChanges();
            }
            return 0;
        }

    }
}

/*    public class TATLogs        
    {
        public int Id { get; set; }
        public int OpdId { get; set; }
        public DateTime TimeAdded { get; set; }
        public String Place { get; set; }
       
    }*/
