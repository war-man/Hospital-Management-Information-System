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
    
    public partial class MaintenanceRequest
    {
        public int Id { get; set; }
        public string ComplainName { get; set; }
        public string Problem { get; set; }
        public string Priority { get; set; }
        public string RequestBuilding { get; set; }
        public string RequestFloor { get; set; }
        public string RequestArea { get; set; }
        public bool Assigned { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime AddedOn { get; set; }
    }
}
