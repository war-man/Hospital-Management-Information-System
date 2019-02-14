using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Pathology.Models
{
    public class MachineDetails
    {
        public string Tlcode { get; set; }
        public string TestName { get; set; }
        public string Type { get; set; }
        public string Parameter_Name { get; set; }
        public string Machine_Name { get; set; }
    }
}