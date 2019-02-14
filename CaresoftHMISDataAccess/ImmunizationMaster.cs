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
    
    public partial class ImmunizationMaster
    {
        public int Id { get; set; }
        public int ImmunizationCategoryId { get; set; }
        public string ImmunizationType { get; set; }
        public string ImmunizationName { get; set; }
        public int LowerAgeInMonths { get; set; }
        public int UpperAgeInMonth { get; set; }
        public string VaccineFlag { get; set; }
        public string BWMYType { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual ImmunizationCategory ImmunizationCategory { get; set; }
        public virtual User User { get; set; }
    }
}