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
    
    public partial class CSSDRequest
    {
        public int Id { get; set; }
        public System.DateTime RequiredDate { get; set; }
        public System.TimeSpan RequiredTime { get; set; }
        public string RequestType { get; set; }
        public string RequestItem { get; set; }
        public string Quantity { get; set; }
        public string Remarks { get; set; }
        public bool Status { get; set; }
        public bool Approval { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime AddedOn { get; set; }
    }
}
