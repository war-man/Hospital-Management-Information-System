using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Radiology.Models
{
    public class SearchPatientModel
    {
        public int PatientCategory { get; set; }
        public int Company { get; set; }
        public int Tarrif { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int Department { get; set; }
        public int Title { get; set; }

        public string DoctorName { get; set; }
        public string PatientRegNo { get; set; }
        public string PatientName { get; set; }

        public String DateRange { get; set; }
        public String patient_type { get; set; }
    }
}