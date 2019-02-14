using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.CustomData
{
    public class ShiftReportData
    {
        public Shift Shift { get; set; }
        public double Consultation { get; set; }
        public double Xray { get; set; }
        public double Labs { get; set; }
        public double Procedure { get; set; }
        public double Drugs { get; set; }
        public List<Shift> Shifts { get; set; }

    }
}