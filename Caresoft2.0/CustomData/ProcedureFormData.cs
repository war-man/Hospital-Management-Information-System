using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.CustomData
{
    public class ProcedureFormData
    {
        public OpdRegister OPDRegister { get; set; }
        public List<Department> Departments { get; set; }
        

    }
}