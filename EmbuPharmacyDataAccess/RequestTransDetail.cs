//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmbuPharmacyDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class RequestTransDetail
    {
        public int ReqTransDetailId { get; set; }
        public Nullable<int> Billno { get; set; }
        public string BatchNo { get; set; }
        public Nullable<double> Rate { get; set; }
        public Nullable<int> Units { get; set; }
        public Nullable<double> FinancialYearId { get; set; }
        public Nullable<bool> StockUpdate { get; set; }
        public string StockUpdatingUser { get; set; }
        public Nullable<bool> billcancelled { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public Nullable<System.DateTime> TransTime { get; set; }
        public Nullable<decimal> DrudId { get; set; }
        public string mfgcomp { get; set; }
        public Nullable<System.DateTime> expdate { get; set; }
        public string barcode { get; set; }
        public Nullable<int> ItemRefund { get; set; }
        public Nullable<int> wardno { get; set; }
        public Nullable<int> currentstock { get; set; }
        public string IssueToFlag { get; set; }
        public string StoreFlag { get; set; }
        public string IssueToType { get; set; }
        public Nullable<int> companyid { get; set; }
        public Nullable<int> Transid { get; set; }
        public string username { get; set; }
        public Nullable<int> Med_Req_id { get; set; }
        public Nullable<int> wardsourceno { get; set; }
        public Nullable<bool> billReturn { get; set; }
        public Nullable<int> ItemId { get; set; }
        public Nullable<int> BranchNo { get; set; }
        public string Remark { get; set; }
        public Nullable<int> DeptID { get; set; }
    }
}
