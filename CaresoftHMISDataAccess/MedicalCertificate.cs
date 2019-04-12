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
    
    public partial class MedicalCertificate
    {
        public int Id { get; set; }
        public Nullable<int> OPDno { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string CertNo { get; set; }
        public string RegNo { get; set; }
        public string DrName { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public Nullable<int> Age { get; set; }
        public string Sex { get; set; }
        public string Location { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Ilness { get; set; }
        public string Advise { get; set; }
        public Nullable<System.DateTime> RestStart { get; set; }
        public Nullable<System.DateTime> RestEnd { get; set; }
        public Nullable<System.DateTime> FitFrom { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
    }
}