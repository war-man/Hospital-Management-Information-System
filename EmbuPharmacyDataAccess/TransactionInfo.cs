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
    
    public partial class TransactionInfo
    {
        public decimal srno { get; set; }
        public Nullable<decimal> billno { get; set; }
        public string pepatid { get; set; }
        public Nullable<System.DateTime> recdate { get; set; }
        public Nullable<double> amount { get; set; }
        public Nullable<decimal> OPDno { get; set; }
        public Nullable<decimal> IPDno { get; set; }
        public Nullable<decimal> ReceiptNo { get; set; }
        public Nullable<bool> Credit { get; set; }
        public Nullable<bool> Cash { get; set; }
        public Nullable<bool> Cheque { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public string BankName { get; set; }
        public Nullable<double> ChequeAmount { get; set; }
        public string CCPersonName { get; set; }
        public string CCNo { get; set; }
        public Nullable<double> cccomm { get; set; }
        public Nullable<double> cccommamount { get; set; }
        public Nullable<System.DateTime> CCExpireDate { get; set; }
        public string CCCompany { get; set; }
        public Nullable<double> CCAmount { get; set; }
        public Nullable<double> CreditAmount { get; set; }
        public Nullable<double> CashAmount { get; set; }
        public Nullable<System.DateTime> tDateTime { get; set; }
        public string ChequeStatus { get; set; }
        public Nullable<double> disc_amount { get; set; }
        public string Disc_Type { get; set; }
        public Nullable<double> tax { get; set; }
        public Nullable<decimal> WardNo { get; set; }
        public string IssueToFlag { get; set; }
        public string IssueToType { get; set; }
        public string StoreFlag { get; set; }
        public string username { get; set; }
        public Nullable<double> FinancialYearId { get; set; }
        public Nullable<decimal> Companyid { get; set; }
        public string Modeofpay { get; set; }
        public Nullable<double> advancepaidincurrency { get; set; }
        public Nullable<decimal> secondryCurrency { get; set; }
        public Nullable<double> discountcurrency { get; set; }
        public string Symbol { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<decimal> corp_compid { get; set; }
        public Nullable<decimal> corp_transno { get; set; }
        public string Type { get; set; }
        public string TransactionType { get; set; }
        public Nullable<bool> Deposite { get; set; }
        public Nullable<double> DepositeAmt { get; set; }
        public Nullable<int> TransId { get; set; }
        public Nullable<int> HIVno { get; set; }
    }
}
