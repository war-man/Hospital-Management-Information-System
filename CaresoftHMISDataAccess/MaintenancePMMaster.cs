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
    
    public partial class MaintenancePMMaster
    {
        public int Id { get; set; }
        public string PMNo { get; set; }
        public string PMTaskName { get; set; }
        public string Remarks { get; set; }
        public int EstLabourHr { get; set; }
        public string myFile { get; set; }
        public string ContractorNo { get; set; }
        public string ContractorName { get; set; }
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public string PartNo { get; set; }
        public string PartDescription { get; set; }
        public string Quantity { get; set; }
        public string SafetyInstructions { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime AddedOn { get; set; }
    }
}
