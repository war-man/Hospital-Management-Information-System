using Caresoft2._0.Areas.Procurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.MedicalStore.ViewModels
{
    public class ExpiryNreorderItems
    {
        public List<ItemMaster> ReorderItems { get; set; }
        public List<ItemMaster> ExpiryItems { get; set; }

    }
}