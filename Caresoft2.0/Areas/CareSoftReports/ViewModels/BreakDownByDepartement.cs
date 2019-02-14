using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.CareSoftReports.ViewModels
{
    public class BreakDownByDepartement
    {
        public double IpCash { get; set; }
        public double Maternity { get; set; }
        public double XRay { get; set; }
        public double Lab { get; set; }
        public double Theatre { get; set; }
        public double Mortuary { get; set; }
        public double OPTreatreatment { get; set; }
        public double Pharmacy { get; set; }
        public double Others { get; set; }
    }
}