//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuantaLabsDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReportExpDate
    {
        public int TransID { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public int ItemCode { get; set; }
        public string ItemName { get; set; }
        public Nullable<double> ReorderLevel { get; set; }
        public Nullable<double> Qty { get; set; }
        public string Unit { get; set; }
        public Nullable<double> TUnit { get; set; }
        public string TransType { get; set; }
        public string IOPD { get; set; }
        public Nullable<int> Regno { get; set; }
        public string TestCode { get; set; }
        public Nullable<System.DateTime> ExpDate { get; set; }
        public Nullable<double> currentqty { get; set; }
        public string SupplierName { get; set; }
        public string BillAmount { get; set; }
        public string PaidAmt { get; set; }
        public string Balance { get; set; }
        public string heading { get; set; }
        public Nullable<int> startline { get; set; }
        public string Inject1 { get; set; }
    }
}