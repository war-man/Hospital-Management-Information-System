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
    
    public partial class PatientAppointment
    {
        public int ID { get; set; }
        public Nullable<int> PatientId { get; set; }
        public string RegNumber { get; set; }
        public int OPD_IPD { get; set; }
        public System.DateTime Date_of_Appointment { get; set; }
        public string Doctor { get; set; }
        public System.TimeSpan AppointmentTime { get; set; }
        public string Explanation { get; set; }
        public int User { get; set; }
        public string AppointmentStatus { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
        public virtual PatientAppointment PatientAppointments1 { get; set; }
        public virtual PatientAppointment PatientAppointment1 { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual User User1 { get; set; }
    }
}