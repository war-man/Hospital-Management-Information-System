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
    
    public partial class HRAppraisalRoleAndObjective
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> StaffId { get; set; }
        public string StaffName { get; set; }
        public string DutyStation { get; set; }
        public string Section { get; set; }
        public string Department { get; set; }
        public string MemosGrade { get; set; }
        public string RolesndResposibilities { get; set; }
        public string Task { get; set; }
        public string ExpectedKPI { get; set; }
        public string AchievedKPI { get; set; }
        public string Comment { get; set; }
        public Nullable<int> Code { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ProposedSolution { get; set; }
        public string Remarks { get; set; }
    }
}
