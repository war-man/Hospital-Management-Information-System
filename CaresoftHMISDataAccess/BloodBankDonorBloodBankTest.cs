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
    
    public partial class BloodBankDonorBloodBankTest
    {
        public int Id { get; set; }
        public int User { get; set; }
        public System.DateTime DateTimeStamp { get; set; }
        public int BloodBag { get; set; }
        public string TestName { get; set; }
        public string TestKit { get; set; }
        public string TestResults { get; set; }
        public string BloodType { get; set; }
        public System.DateTime TestingDate { get; set; }
        public System.TimeSpan TestingTime { get; set; }
        public int Technician { get; set; }
        public int MedicalOfficer { get; set; }
        public Nullable<System.DateTime> DiscardDate { get; set; }
    }
}
