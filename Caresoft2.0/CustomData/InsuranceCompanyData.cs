using CaresoftHMISDataAccess
    ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class InsuranceCompanyData
    {
        public Company InsuranceCompany { get; set; }
        public List<Company> InsuranceCompanies { get; set; }
    }
}