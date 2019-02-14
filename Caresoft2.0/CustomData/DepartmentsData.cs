using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class DepartmentsData
    {
        public List<Department> Departments { get; set; }
        public List<DepartmentType> DepartmentTypeList { get; set; }
        public Department Department { get; set; }
        
    }
}