using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class AddUserUserManagementData
    {
        public List<Department> lstDepartments { get; set; }
        public List<Employee> lstEmployee { get; set; }
        public List<UserRole> lstUserRole { get; set; }
    }
}