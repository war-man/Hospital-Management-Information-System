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
    
    public partial class InvoiceNumber
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> TDateTime { get; set; }
        public double FinancialYearId { get; set; }
        public Nullable<double> InvoiceAmount { get; set; }
        public string IssueToFlag { get; set; }
        public string StoreFlag { get; set; }
        public Nullable<int> companyid { get; set; }
        public string TempInvoiceNo { get; set; }
        public string UserName { get; set; }
    }
}
