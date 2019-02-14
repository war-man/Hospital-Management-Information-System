using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class AddRolesRight
    {
        public int ActionId { get; set; }
        public int UserRoleId { get; set; }
        public DateTime dateAdded { get; set; }
        public string Status { get; set; }
    }

    public class AddRolesRightRefined
    {
        public int RoleRightId { get; set; }
        public int UserRoleId { get; set; }
        public string Status { get; set; }
    }
}