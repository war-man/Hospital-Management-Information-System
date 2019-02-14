using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;
using PagedList;

namespace Caresoft2._0.CustomData
{
    public class ServicesMasterData
    {
        public Service Service { get; set; }
        public IPagedList<Service> Services { get; set; }
        public List<ServiceGroup> ServiceGroups { get; set; }
        public List<Department> Departments { get; set; }
    }
}