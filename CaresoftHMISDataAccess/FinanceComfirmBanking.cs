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
    
    public partial class FinanceComfirmBanking
    {
        public int Id { get; set; }
        public System.DateTime DateComfirmed { get; set; }
        public string SlipNo { get; set; }
        public System.DateTime SlipDate { get; set; }
        public int BankingID { get; set; }
        public int User { get; set; }
    
        public virtual FinanceBanking FinanceBanking { get; set; }
    }
}
