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
    
    public partial class FinanceDirectBanking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PaymentMode { get; set; }
        public Nullable<int> ChequeNo { get; set; }
        public double Amount { get; set; }
        public string GIName { get; set; }
        public int AccNo { get; set; }
        public string Branch { get; set; }
        public System.DateTime Date { get; set; }
    }
}
