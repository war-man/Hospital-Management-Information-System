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
    
    public partial class RenalDialysisOrder
    {
        public int Id { get; set; }
        public int OPDNo { get; set; }
        public System.TimeSpan TreatmentTime { get; set; }
        public string Priming { get; set; }
        public string DialysisSolution { get; set; }
        public string Dialyzer { get; set; }
        public string BathK { get; set; }
        public string MembraneType { get; set; }
        public string Heparinization { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
    }
}