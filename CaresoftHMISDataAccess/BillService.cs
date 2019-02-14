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
    
    public partial class BillService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BillService()
        {
            this.BillAdjustmentLogs = new HashSet<BillAdjustmentLog>();
            this.InvoiceServiceAllocations = new HashSet<InvoiceServiceAllocation>();
            this.IPDBillPartialPayments = new HashSet<IPDBillPartialPayment>();
            this.RefundedItems = new HashSet<RefundedItem>();
        }
    
        public int Id { get; set; }
        public int OPDNo { get; set; }
        public int DepartmentId { get; set; }
        public int SeviceId { get; set; }
        public string ServiceName { get; set; }
        public double Price { get; set; }
        public double Quatity { get; set; }
        public int TariffId { get; set; }
        public double Award { get; set; }
        public Nullable<double> WaivedAmount { get; set; }
        public Nullable<int> WaiverNo { get; set; }
        public double DoctorFee { get; set; }
        public bool Paid { get; set; }
        public Nullable<int> BillNo { get; set; }
        public Nullable<int> WorkOrderTestId { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public bool Offered { get; set; }
        public System.DateTime DateAdded { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<int> OfferedBy { get; set; }
        public bool IsNurse { get; set; }
        public Nullable<double> Discount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillAdjustmentLog> BillAdjustmentLogs { get; set; }
        public virtual BillPayment BillPayment { get; set; }
        public virtual OpdRegister OpdRegister { get; set; }
        public virtual OpdRegister OpdRegister1 { get; set; }
        public virtual Service Service { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceServiceAllocation> InvoiceServiceAllocations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IPDBillPartialPayment> IPDBillPartialPayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RefundedItem> RefundedItems { get; set; }
    }
}