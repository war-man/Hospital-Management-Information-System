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
    
    public partial class Parameter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Parameter()
        {
            this.NormalValues = new HashSet<NormalValue>();
            this.PanicValues = new HashSet<PanicValue>();
            this.WorkOrderTestParameters = new HashSet<WorkOrderTestParameter>();
        }
    
        public int Id { get; set; }
        public string Parameter_Name { get; set; }
        public string Parameter_Code { get; set; }
        public Nullable<int> Parameter_Order { get; set; }
        public string Parameter_Method { get; set; }
        public Nullable<int> Machine_Name { get; set; }
        public string Title_Machine_Name { get; set; }
        public string Short_Form { get; set; }
        public Nullable<int> Heading { get; set; }
        public Nullable<int> Test { get; set; }
        public int DepartmentRadPath { get; set; }
        public int BranchId { get; set; }
    
        public virtual LabTest LabTest { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NormalValue> NormalValues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PanicValue> PanicValues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkOrderTestParameter> WorkOrderTestParameters { get; set; }
    }
}
