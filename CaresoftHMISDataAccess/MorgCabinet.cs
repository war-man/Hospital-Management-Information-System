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
    
    public partial class MorgCabinet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MorgCabinet()
        {
            this.MorgAdmissions = new HashSet<MorgAdmission>();
        }
    
        public int Id { get; set; }
        public int ColdRoomId { get; set; }
        public string CabinetName { get; set; }
        public Nullable<int> AuthorizedBodies { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MorgAdmission> MorgAdmissions { get; set; }
        public virtual MorgColdroom MorgColdroom { get; set; }
    }
}