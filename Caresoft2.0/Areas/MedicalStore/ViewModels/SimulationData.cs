using Caresoft2._0.Areas.Procurement.Models;
using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.MedicalStore.ViewModels
{
    public class SimulationData
    {
        public OpdRegister opdRegister { get; set; }
        public SimulationPatientIssueVoucher SimulationPatientIssueVoucher { get; set; }
        public List<SimulationTreatment> LstSimulationTreatment { get; set; }
    }
}