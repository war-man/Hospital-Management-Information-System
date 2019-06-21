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
    
    public partial class InvoiceServiceAllocation
    {
        public int Id { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public int BillServiceId { get; set; }
        public string ServiceName { get; set; }
        public double AmountAllocated { get; set; }
        public int BranchId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> MedicationId { get; set; }
    
        public virtual BillService BillService { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual InsuranceInvoice InsuranceInvoice { get; set; }
    }
}
