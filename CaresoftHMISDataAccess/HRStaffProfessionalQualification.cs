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
    
    public partial class HRStaffProfessionalQualification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string StaffId { get; set; }
        public string Course { get; set; }
        public Nullable<System.DateTime> DateOfQualification { get; set; }
        public string CourseTitle { get; set; }
        public string Institution { get; set; }
        public string Grade { get; set; }
        public string Remarks { get; set; }
        public string Filename { get; set; }
    }
}
