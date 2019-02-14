using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.CustomData
{
    public class WardTypeChargesData { 
        public List<HSWardCategory> WardCategories { get; set; }
        public List<Service> IPDServices { get; set; }

    }
}