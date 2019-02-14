using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class EmployeeAndLstUserRoles
    {
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string password { get; set; }
        public List<allRoles> userRoles { get; set; }
    }

    public class allRoles
    {
        public int Id { get; set; }
        public string roleName { get; set; }
    }
}