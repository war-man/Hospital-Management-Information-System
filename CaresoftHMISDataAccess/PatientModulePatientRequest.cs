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
    
    public partial class PatientModulePatientRequest
    {
        public int Id { get; set; }
        public System.DateTime DateTimeStamp { get; set; }
        public string RegNo { get; set; }
        public string PatientName { get; set; }
        public int BedNo { get; set; }
        public string Ward { get; set; }
        public string Room { get; set; }
        public string MealType { get; set; }
        public string MealTime { get; set; }
        public string LinenType { get; set; }
        public string Others { get; set; }
        public Nullable<int> LinenQuantity { get; set; }
        public Nullable<int> MealQuantity { get; set; }
        public Nullable<bool> Approved { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
