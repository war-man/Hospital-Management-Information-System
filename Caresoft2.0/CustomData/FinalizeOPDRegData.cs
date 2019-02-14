using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class FinalizeOPDRegData
    {
        public int OPDNo { get; set; }
        public String ConsultantDoctor { get; set; }
        public String ReferringDoctor { get; set; }
        public String BroughtBy { get; set; }
        public String MLCNo { get; set; }
        public String PoliceStationName { get; set; }
        public String ReferringFacility { get; set; }
        public bool ReferralIn { get; set; }
        public bool Counterreferral { get; set; }
        public bool ReferralByCHW { get; set; }
        public String Status { get; set; }
        public String Department { get; set; }
        public String MainCategory { get; set; }
        public int PatientCategory { get; set; }

    }
}