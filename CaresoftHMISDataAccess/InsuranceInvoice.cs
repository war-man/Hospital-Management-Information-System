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
    
    public partial class InsuranceInvoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InsuranceInvoice()
        {
            this.InvoiceServiceAllocations = new HashSet<InvoiceServiceAllocation>();
        }
    
        public int id { get; set; }
        public int OpdId { get; set; }
        public Nullable<int> NhifRebate { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public Nullable<double> NhifDifference { get; set; }
        public int CompanyId { get; set; }
        public string Batch { get; set; }
        public int BranchId { get; set; }
        public bool Finalized { get; set; }
        public int UserId { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public Nullable<bool> Preouth { get; set; }
    
        public virtual Branch Branch { get; set; }
        public virtual Company Company { get; set; }
        public virtual OpdRegister OpdRegister { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceServiceAllocation> InvoiceServiceAllocations { get; set; }
    }
}