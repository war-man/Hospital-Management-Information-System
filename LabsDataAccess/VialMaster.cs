//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LabsDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class VialMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VialMaster()
        {
            this.Test_Turn_Around_Times = new HashSet<Test_Turn_Around_Times>();
        }
    
        public int Id { get; set; }
        public string Vial_Type { get; set; }
        public Nullable<System.DateTime> CreatedUtc { get; set; }
        public int DepartmentRadPath { get; set; }
        public int BranchId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test_Turn_Around_Times> Test_Turn_Around_Times { get; set; }
    }
}
