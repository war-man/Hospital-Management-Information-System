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
    
    public partial class MedicalHistoryRespons
    {
        public int Id { get; set; }
        public int OPDNo { get; set; }
        public int MedicalHistoryQuestionId { get; set; }
        public string Response { get; set; }
        public int BranchId { get; set; }
        public int UserId { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual MedicalHistoryQuestion MedicalHistoryQuestion { get; set; }
        public virtual OpdRegister OpdRegister { get; set; }
    }
}
