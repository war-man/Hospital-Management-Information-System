using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class AssignRoleObj
    {
        public List<UserRole> userRoles { get; set; }
        public List<string> controllers { get; set; }
        public List<Department> departments { get; set; }
    }
}