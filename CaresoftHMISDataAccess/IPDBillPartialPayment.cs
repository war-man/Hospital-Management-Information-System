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
    
    public partial class IPDBillPartialPayment
    {
        public int Id { get; set; }
        public Nullable<int> BillServiceId { get; set; }
        public Nullable<int> MedicationId { get; set; }
        public double AllocatedAmount { get; set; }
        public int BillPaymentNo { get; set; }
        public Nullable<int> PaymentMode { get; set; }
    
        public virtual BillPayment BillPayment { get; set; }
        public virtual BillService BillService { get; set; }
        public virtual Medication Medication { get; set; }
    }
}
