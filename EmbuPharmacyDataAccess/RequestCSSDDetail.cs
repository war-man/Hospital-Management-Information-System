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
    
    public partial class RequestCSSDDetail
    {
        public int ReqDetId { get; set; }
        public Nullable<int> ReqId { get; set; }
        public Nullable<int> DrugId { get; set; }
        public Nullable<int> ReqQty { get; set; }
        public Nullable<int> IssueQty { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string StoreFlag { get; set; }
        public string IssueToFlag { get; set; }
    
        public virtual RequestCSSDMain RequestCSSDMain { get; set; }
    }
}