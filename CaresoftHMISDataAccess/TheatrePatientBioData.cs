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
    
    public partial class TheatrePatientBioData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TheatrePatientBioData()
        {
            this.TheatreOperationPersonels = new HashSet<TheatreOperationPersonel>();
        }
    
        public int Id { get; set; }
        public int PatientOPDIPD { get; set; }
        public string PatientName { get; set; }
        public int InternalRefferal { get; set; }
        public string HIV { get; set; }
        public int DepartmentId { get; set; }
        public System.DateTime AppointmentDate { get; set; }
        public string RegStatus { get; set; }
        public Nullable<int> FacilityId { get; set; }
        public Nullable<int> UserId { get; set; }
        public System.DateTime CreatedUTC { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
        public virtual TheatreDepartment TheatreDepartment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TheatreOperationPersonel> TheatreOperationPersonels { get; set; }
        public virtual User User { get; set; }
    }
}
