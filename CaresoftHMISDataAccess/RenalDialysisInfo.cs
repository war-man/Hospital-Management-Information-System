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
    
    public partial class RenalDialysisInfo
    {
        public int Id { get; set; }
        public int OPDNo { get; set; }
        public System.TimeSpan Time { get; set; }
        public string BP { get; set; }
        public string Pulse { get; set; }
        public int Temp { get; set; }
        public string BFR { get; set; }
        public string TMP { get; set; }
        public string UF { get; set; }
        public string VP { get; set; }
        public string DP { get; set; }
        public string DR { get; set; }
        public string DialTemp { get; set; }
        public string Heparin { get; set; }
        public string ProSulp { get; set; }
        public string Fluids { get; set; }
        public System.TimeSpan CoagTime { get; set; }
        public string Comments { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
    }
}
