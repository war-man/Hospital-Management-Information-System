using Caresoft2._0.Areas.Procurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.MedicalStore.ViewModels
{
    public class GenericDrugViewModel
    {
        public GenericDrugName genericDrugName { get; set; }
        public int[] SideEffects { get; set; }
        public int[] Toxicities { get; set; }
        public int[] Allergies { get; set; }
        public int[] Contraindication { get; set; }
    }
}