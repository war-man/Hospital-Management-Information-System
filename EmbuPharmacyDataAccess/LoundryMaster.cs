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
    
    public partial class LoundryMaster
    {
        public int loundry_id { get; set; }
        public string loundryName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string TelePhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public string StoreFlag { get; set; }
        public string IssueToFlag { get; set; }
        public Nullable<int> companyid { get; set; }
    }
}
