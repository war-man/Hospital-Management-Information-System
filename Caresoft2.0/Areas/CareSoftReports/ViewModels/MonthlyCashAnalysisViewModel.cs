using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.CareSoftReports.ViewModels
{
    public class MonthlyCashAnalysisViewModel
    {
        public BreakDownByDepartement breakDownByDepartement { get; set; }
        public CashReceipts cashReceipts { get; set; }
        public RevenueNotCollected revenueNotCollected { get; set; }
        public Banking banking { get; set; }
        public CategoryMonthlyCashAnalysis category { get; set; }
    }
}