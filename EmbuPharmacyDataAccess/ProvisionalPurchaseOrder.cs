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
    
    public partial class ProvisionalPurchaseOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProvisionalPurchaseOrder()
        {
            this.AcceptedProvisionalPurchaseOrders = new HashSet<AcceptedProvisionalPurchaseOrder>();
            this.ProvisionalPurchaseOrderItemsDetails = new HashSet<ProvisionalPurchaseOrderItemsDetail>();
            this.ProvisionalPurchaseOrderSupplierDetails = new HashSet<ProvisionalPurchaseOrderSupplierDetail>();
        }
    
        public int ProvisionalPOId { get; set; }
        public Nullable<System.DateTime> ProvisionalPODate { get; set; }
        public string Description { get; set; }
        public double FinancialYearId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string StoreFlag { get; set; }
        public string IssueToFlag { get; set; }
        public string UserName { get; set; }
        public Nullable<bool> IsDonor { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AcceptedProvisionalPurchaseOrder> AcceptedProvisionalPurchaseOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProvisionalPurchaseOrderItemsDetail> ProvisionalPurchaseOrderItemsDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProvisionalPurchaseOrderSupplierDetail> ProvisionalPurchaseOrderSupplierDetails { get; set; }
    }
}
