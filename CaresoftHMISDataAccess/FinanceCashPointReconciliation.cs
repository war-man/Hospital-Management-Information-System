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
    
    public partial class FinanceCashPointReconciliation
    {
        public int ID { get; set; }
        public int ShiftId { get; set; }
        public double CashActualAmount { get; set; }
        public double ChequeActualAmount { get; set; }
        public double MpesaActualAmount { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime TimeAdded { get; set; }
        public string ChequeNumbers { get; set; }
    }
}