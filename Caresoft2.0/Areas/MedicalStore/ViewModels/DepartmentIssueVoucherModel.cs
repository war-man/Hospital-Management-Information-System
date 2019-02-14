using Caresoft2._0.Areas.Procurement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.MedicalStore.ViewModels
{
    public class DepartmentIssueVoucherModel
    {
        public int CurrentStock { get; set; }
        public int ItemMasterId { get; set; }
        public int Units { get; set; }
        public decimal Amount { get; set; }
        public int DepartmentId { get; set; }
    }
}