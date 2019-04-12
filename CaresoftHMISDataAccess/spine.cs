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
    
    public partial class spine
    {
        public int Id { get; set; }
        public int OPDno { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public string CervicalAnky { get; set; }
        public string CervicalFl { get; set; }
        public string CervicalEXT { get; set; }
        public string CervicalRT_ROT { get; set; }
        public string CervicalLT_ROT { get; set; }
        public string CervicalRT_FL { get; set; }
        public string CervicalLT_FL { get; set; }
        public string ThoracicAnky { get; set; }
        public string ThoraciclFl { get; set; }
        public string ThoracicEXT { get; set; }
        public string ThoracicRT_ROT { get; set; }
        public string ThoracicLT_ROT { get; set; }
        public string ThoracicRT_FL { get; set; }
        public string ThoracicLT_FL { get; set; }
        public string LumbarAnky { get; set; }
        public string LumbarlFl { get; set; }
        public string LumbarEXT { get; set; }
        public string LumbarRT_ROT { get; set; }
        public string LumbarLT_ROT { get; set; }
        public string LumbarRT_FL { get; set; }
        public string LumbarLT_FL { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string SchobersTest { get; set; }
        public string Straddle { get; set; }
        public string ChestExpansion { get; set; }
        public string OccputDistance { get; set; }
        public string LeteralBending { get; set; }
        public string Notes { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
    }
}