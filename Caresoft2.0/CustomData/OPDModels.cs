using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class OPDModels
    {
        public List<Employee> Doctors { get; set; }
        public List<Department> RevenueDepartments { get; set; }
        public Patient Patient { get; set; }
        //public List<Company> PatientCategories { get; set; }
        public List<CompanyType> MainCategories { get; set; }
        public List<Relationship> Relationships { get; set; }
        public Service Consultation { get; set; }
        public OpdRegister OPDEntry { get; set; }

    }
}