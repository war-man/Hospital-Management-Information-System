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
    
    public partial class IPDDischargeType
    {
        public int Id { get; set; }
        public string PatientReg { get; set; }
        public string DischargeType { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual IPDDischargeType IPDDischargeType1 { get; set; }
        public virtual IPDDischargeType IPDDischargeType2 { get; set; }
    }
}
