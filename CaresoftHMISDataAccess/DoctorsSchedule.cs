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
    
    public partial class DoctorsSchedule
    {
        public int ID { get; set; }
        public int Doctor { get; set; }
        public string Speciality { get; set; }
        public string Shift { get; set; }
        public System.TimeSpan TimeFrom { get; set; }
        public System.TimeSpan ToTime { get; set; }
        public int Interval { get; set; }
        public string Days { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}