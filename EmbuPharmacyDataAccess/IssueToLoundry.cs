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
    
    public partial class IssueToLoundry
    {
        public int LoundryIssueId { get; set; }
        public Nullable<int> loundry_id { get; set; }
        public string DrugName { get; set; }
        public string BatchNo { get; set; }
        public Nullable<int> DrugId { get; set; }
        public Nullable<int> QuantityIssued { get; set; }
        public Nullable<int> NoOfCleanCloths { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public Nullable<bool> ReceivedStatus { get; set; }
        public Nullable<int> NoOfDamagedCloths { get; set; }
        public string StoreFlag { get; set; }
        public string IssueToFlag { get; set; }
        public Nullable<int> companyid { get; set; }
    }
}
