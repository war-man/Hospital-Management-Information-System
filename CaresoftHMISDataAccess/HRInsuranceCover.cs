//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CaresoftHMISDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class HRInsuranceCover
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> BenefitId { get; set; }
        public string Description { get; set; }
        public string Comments { get; set; }
        public Nullable<int> PolicyId { get; set; }
        public string PolicyDescription { get; set; }
        public string CeilingAmount { get; set; }
        public string InstitutionsOfferingCover { get; set; }
        public Nullable<int> ClaimId { get; set; }
        public string ClaimDescription { get; set; }
        public string CompanyToClaimFrom { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> EligibilityId { get; set; }
        public string EligibilityStatus { get; set; }
        public string PolicyDesription1 { get; set; }
        public string StaffNames { get; set; }
        public Nullable<int> FundId { get; set; }
        public string FundDescription { get; set; }
        public string WhereToClaimFrom { get; set; }
        public string Remarks1 { get; set; }
        public string PolicyName { get; set; }
        public string Brokers { get; set; }
    }
}
