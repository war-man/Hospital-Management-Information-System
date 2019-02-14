using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;
namespace Caresoft2._0.CustomData
{
    public class AdmissionFormData
    {
        public HSBed Bed { get; set; }
        public List<CompanyType> PatientMainCategories { get; set; }

    }
}