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
    
    public partial class RequestForNewItem
    {
        public int RequestId { get; set; }
        public string DrugName { get; set; }
        public Nullable<int> DrugId { get; set; }
        public Nullable<double> ReqQty { get; set; }
        public Nullable<int> issue_qty { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<bool> ApprovedReqflag { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public string ApprovedBy { get; set; }
        public string UserName { get; set; }
        public string StoreFlag { get; set; }
        public string IssueToFlag { get; set; }
        public Nullable<int> CompanyId { get; set; }
    }
}