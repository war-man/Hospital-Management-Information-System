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
    
    public partial class MaintenanceScheduling
    {
        public int Id { get; set; }
        public string PMNo { get; set; }
        public string PMName { get; set; }
        public string Schedule { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.TimeSpan FromTime { get; set; }
        public System.DateTime DueOn { get; set; }
        public string WorkType { get; set; }
        public string WorkTrade { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime AddedOn { get; set; }
        public bool Status { get; set; }
    }
}
