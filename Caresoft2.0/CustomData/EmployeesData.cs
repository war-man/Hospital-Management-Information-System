using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class EmployeesData
    {
        public List<Employee> Employess { get; set; }
        public Employee Employee { get; set; }
        public List<Designation> DesignationsList { get; set; }
    }
}