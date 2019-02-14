using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.CareSoftReports.ViewModels
{
    public class RevenueNotCollected
    {
        public double NhifPatients { get; set; }
        public double OtherDebtors { get; set; }
        public double Waivers  { get; set; }
        public double Exemptions { get; set; }
        public double Absconders { get; set; }
    }
}