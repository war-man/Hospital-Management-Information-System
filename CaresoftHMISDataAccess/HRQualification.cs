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
    
    public partial class HRQualification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime DateAdded { get; set; }
        public int ApplicantId { get; set; }
        public string EducationLevel { get; set; }
        public string QualificationsAttained { get; set; }
        public string GradeObtained { get; set; }
        public string InstitutionAttended { get; set; }
        public Nullable<decimal> GraduationYear { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> DateOfQualification { get; set; }
        public string Institution { get; set; }
        public string TitleCourse { get; set; }
        public string Grade { get; set; }
        public string Remarks1 { get; set; }
    }
}
