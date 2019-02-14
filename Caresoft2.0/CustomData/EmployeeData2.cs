using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class EmployeeData2
    {
       
        [Key]
        public Employee Employee { get; set; }
        public List<Employee> Employess { get; set; }
        public List<Designation> DesignationsList { get; set; }
        public List<Department> LstDepartment { get; set; }
  
    }
}