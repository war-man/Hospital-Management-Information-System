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
    
    public partial class HRContractManagement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> ContractId { get; set; }
        public Nullable<int> StaffId { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public Nullable<System.DateTime> ExpectedExpiryDate { get; set; }
        public Nullable<int> AccessControlPassId { get; set; }
        public string AccessControlPassStatus { get; set; }
        public Nullable<double> ContractualAmount { get; set; }
        public Nullable<double> MonthlyPayment { get; set; }
        public string FinalDuesForPayment { get; set; }
        public Nullable<double> FinalPaymentAmount { get; set; }
        public Nullable<bool> Renewable { get; set; }
        public Nullable<System.DateTime> FinalDateOfPayment { get; set; }
        public string RequirementsForContract { get; set; }
        public Nullable<System.DateTime> BeginDate { get; set; }
    }
}
