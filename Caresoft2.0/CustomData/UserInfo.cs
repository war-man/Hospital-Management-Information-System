using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class UserInfo
    {
        public int deptId { get; set; }
        public int employeeId { get; set; }
        public string Email { get; set; }
        public string phoneNumber { get; set; }
        public int userRoleId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}