using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Accounting.Models
{
    public class Invoice
    {
        
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public string FinancialYear { get; set; }
        public string SupplierType { get; set; }
        public string PONumber { get; set; }
        public System.DateTime PODate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string TempInvoiceNumber { get; set; }
        public string Username { get; set; }
        public Nullable<System.DateTime> TDateTime { get; set; }
        public double AccFlag { get; set; }
        public double InvoiceAmount { get; set; }
        public string CompName { get; set; }
        public string IssueToFlag { get; set; }
        public string StoreFlag { get; set; }
        public int companyid { get; set; }
        public double InvoiceDiscount { get; set; }
        public string Remark { get; set; }
        public string FinalInvoiceNo { get; set; }
        public bool cflag { get; set; }
        public bool FinalApproval { get; set; }
        public string ApprovedBy { get; set; }
        public string DisApprovedBy { get; set; }
        public bool IsDonor { get; set; }
    
        
    }
    public class InvoiceApproval
    {
        public int Id { get; set; }
        public int InvoiceNo { get; set; }
        public int ApprovalAmount { get; set; }
        public System.DateTime DateAdded { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<double> InvoiceAmount { get; set; }
    }
    public class General
    {
        public  Invoice Invoice { get; set;  }
        public InvoiceApproval InvoiceApproval { get; set; }
    }
}