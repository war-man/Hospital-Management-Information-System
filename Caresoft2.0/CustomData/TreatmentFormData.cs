using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caresoft2._0.Areas.Procurement.Models;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.CustomData
{
    public class TreatmentFormData
    {
        public OpdRegister OPDRegister { get; set; }
        public Complaint Complaint { get; set; }

        public List<User> Users { get; set; }
        public List<RoutingAdmin> RoutingAdmins { get; set; }
        public List<Dose> Frequencies { get; set; }
        
    }
}