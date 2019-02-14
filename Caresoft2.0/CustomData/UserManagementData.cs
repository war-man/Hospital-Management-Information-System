using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class UserManagementData
    {
        public List<User> lstUsers { get; set; }
        public List<Department> lstDepartments { get; set; }
    }
}