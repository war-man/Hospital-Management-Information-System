using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class ServicesPricesData
    {
        public List<ServicesPrice> ServicesPrices { get; set; }
        public List<AlteredPrice> AlteredPrices { get; set; }
        public int TariffId { get; set; }
    }
    public class AlteredPrice
    {
        public int ServiceId { get; set; }
        public double CashPrice { get; set; }
    }
}