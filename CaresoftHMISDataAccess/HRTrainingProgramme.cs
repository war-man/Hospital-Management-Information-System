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
    
    public partial class HRTrainingProgramme
    {
        public int Id { get; set; }
        public Nullable<int> ProgrammeId { get; set; }
        public string Department { get; set; }
        public string Sections { get; set; }
        public string TrainingPeriod { get; set; }
        public string ProgrammeDescription { get; set; }
        public string Sponsor { get; set; }
        public int BranchId { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
    }
}