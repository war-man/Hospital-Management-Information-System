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
    
    public partial class HRManagement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> ManagementLevelId { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string MainManagementLevel { get; set; }
        public Nullable<int> AuthorityId { get; set; }
        public string AuthorityDescription { get; set; }
        public Nullable<int> ManagementCode { get; set; }
        public string ManagementLevelDesignation { get; set; }
        public string Comments { get; set; }
        public string ManagementLevel { get; set; }
    }
}
