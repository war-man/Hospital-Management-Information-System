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
    
    public partial class BillTransactionold
    {
        public int BillNo { get; set; }
        public Nullable<System.DateTime> BillDate { get; set; }
        public Nullable<decimal> pepatid { get; set; }
        public Nullable<decimal> OPDno { get; set; }
        public Nullable<decimal> IPDno { get; set; }
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
        public Nullable<double> TotalAmount { get; set; }
        public Nullable<double> Amountreceivd { get; set; }
        public Nullable<double> AmtDue { get; set; }
        public string BillFlag { get; set; }
        public Nullable<double> AccFlag { get; set; }
        public string username { get; set; }
        public Nullable<System.DateTime> tDateTime { get; set; }
        public double FinancialYearId { get; set; }
        public string ChequeStatus { get; set; }
        public string Computername { get; set; }
        public string pname { get; set; }
        public string drname { get; set; }
        public Nullable<double> disc_amount { get; set; }
        public string Disc_Type { get; set; }
        public Nullable<double> tax { get; set; }
        public Nullable<decimal> WardNo { get; set; }
        public string IssueToFlag { get; set; }
        public string dept { get; set; }
        public string sex { get; set; }
        public Nullable<int> Age { get; set; }
        public string IssueToType { get; set; }
        public string StoreFlag { get; set; }
        public Nullable<int> companyid { get; set; }
        public string Modeofpay { get; set; }
        public Nullable<double> advancepaidincurrency { get; set; }
        public Nullable<decimal> secondryCurrency { get; set; }
        public Nullable<double> discountcurrency { get; set; }
        public string CurrencyName { get; set; }
        public string Symbol { get; set; }
        public Nullable<decimal> corp_compid { get; set; }
        public Nullable<decimal> corp_transno { get; set; }
        public string PaymentMode { get; set; }
        public string Remarks { get; set; }
        public string PatientCategory { get; set; }
    }
}
