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
    
    public partial class HRRecruitmentProcessing
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public string SearchingCriteria { get; set; }
        public string RequisitionNo { get; set; }
        public string EducationalQualifications { get; set; }
        public string ProfessionalQualificatons { get; set; }
        public string Experiences { get; set; }
        public Nullable<int> FileId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string RequisitionStatus { get; set; }
    }
}
