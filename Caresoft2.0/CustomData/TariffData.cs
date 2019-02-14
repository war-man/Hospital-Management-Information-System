using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class TariffData
    {
        public List<Company> CompaniesList { get; set; }
        public Tariff Tariff { get; set; }
        public List<Tariff> Tariffs { get; set; }
        public List<Department> DepartmentsList { get; set; }
        public List<Service> Services { get; set; }
        public ServicesPrice ServicePrice { get; set; }
        public List<ServicesPrice> ServicePrices { get; set; }
    }
}