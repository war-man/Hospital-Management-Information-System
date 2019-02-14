using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class ServicesData
    {
        public List<Department> DepartmentsList { get; set; }
        public List<Service> Services { get; set; }
        public Service Service { get; set; }
    }
}