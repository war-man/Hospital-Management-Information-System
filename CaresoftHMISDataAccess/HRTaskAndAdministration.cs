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
    
    public partial class HRTaskAndAdministration
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> TaskId { get; set; }
        public string NewTask { get; set; }
        public string RetrieveTask { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string DutyStation { get; set; }
        public string TaskDescription { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> ExpectedCompletionDate { get; set; }
        public string StaffResposibleForTaskExecution { get; set; }
        public string MeetingVenues { get; set; }
    }
}
