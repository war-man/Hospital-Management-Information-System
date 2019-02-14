using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.CareSoftReports.ViewModels
{
    public class PatientListDataModel
    {
        public List<Employee> Employees { get; set; }
        public List<Department> Departments { get; set; }
        public List<Company> Companies { get; set; }
    }
}