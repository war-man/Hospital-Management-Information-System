//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmbuPharmacyDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class FloorMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FloorMaster()
        {
            this.FloorAreaMasters = new HashSet<FloorAreaMaster>();
        }
    
        public int FloorId { get; set; }
        public Nullable<int> BuildingID { get; set; }
        public string FloorName { get; set; }
        public Nullable<int> companyid { get; set; }
    
        public virtual BuildingMaster BuildingMaster { get; set; }
        public virtual BuildingMaster BuildingMaster1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FloorAreaMaster> FloorAreaMasters { get; set; }
    }
}
