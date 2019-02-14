using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;
namespace Caresoft2._0.CustomData
{
    public class DiagnosisFormData
    {
        public  OpdRegister OpdRegister { get; set; }
        public List<Diagnosis1s> Diagnosis1s { get; set; }
    }
}