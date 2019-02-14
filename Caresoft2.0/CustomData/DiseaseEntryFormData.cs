using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.CustomData
{
    public class DiseaseEntryFormData
    {
        public List<Disease> Diseases { get; set; }
        public OpdRegister OpdRegister { get; set; }
    }
}