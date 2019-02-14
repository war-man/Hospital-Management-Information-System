using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.PharmacyModule.ViewModel
{
    public class ItemLocationModel
    {
        public int ItemId { get; set; }
        public string RackName { get; set; }
        public int CellInRack { get; set; }
    }
}