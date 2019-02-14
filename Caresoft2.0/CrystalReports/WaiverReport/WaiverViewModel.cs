using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CrystalReports.WaiverReport
{
    public class WaiverViewModel
    {
        public DateTime DateAdded { get; set; }
        public string PatientName { get; set; }
        public double AmountWaived { get; set; }
        public string WaivedBy { get; set; }
        public string Reason { get; set; }
    }
}