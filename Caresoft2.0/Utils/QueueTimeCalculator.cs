using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Utils
{
    public class QueueTimeCalculator
    {

        DateTime? TimeAdded;
        DateTime? QueueTime;

        public QueueTimeCalculator(int opdid)
        {
            var opd = new CaresoftHMISEntities().OpdRegisters.Find(opdid);

            if (opd != null)
            {
                TimeAdded = opd.TimeAdded;
                QueueTime = opd.QueueTime;
            }
        }

        DateTime NowTime = DateTime.Now;

        public TimeSpan? TotalHours
        {
            get
            {
                return (this.NowTime - this.TimeAdded);
            }
        }

        public TimeSpan? BillingQueue
        {
            get
            {
                return (this.NowTime - this.QueueTime);
            }
        }
    }
}