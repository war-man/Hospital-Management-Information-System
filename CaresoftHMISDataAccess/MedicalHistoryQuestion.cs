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
    
    public partial class MedicalHistoryQuestion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MedicalHistoryQuestion()
        {
            this.MedicalHistoryResponses = new HashSet<MedicalHistoryRespons>();
        }
    
        public int Id { get; set; }
        public string MedicalHistoryQuestionName { get; set; }
        public int BranchId { get; set; }
        public int UserId { get; set; }
        public System.DateTime TimeAdded { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicalHistoryRespons> MedicalHistoryResponses { get; set; }
    }
}
