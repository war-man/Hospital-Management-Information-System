//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuantaLabsDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class LedgerTransaction
    {
        public int LedgerId { get; set; }
        public string PSCNo { get; set; }
        public Nullable<System.DateTime> RegDate { get; set; }
        public string ParticularField { get; set; }
        public Nullable<double> CreditAmt { get; set; }
        public Nullable<double> DebitAmt { get; set; }
        public Nullable<int> BillNo { get; set; }
        public string BillFormat { get; set; }
        public Nullable<int> companyid { get; set; }
    }
}
