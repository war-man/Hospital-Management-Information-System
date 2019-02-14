using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Procurement.Models
{
    public class ProcurementModels
    {
    }

    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string StoreName { get; set; }
    }

    public class GenericDrugName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        public string InvestigationOfAdvice { get; set; }

        public ICollection<Drug> drugs { get; set; }
        public ICollection<GenericNameSideEffects> SideEffects { get; set; }
        public ICollection<GenericNameToxicites> Toxicites { get; set; }
        public ICollection<GenericNameContraindication> Contraindication { get; set; }
        public ICollection<GenericNameAllergies> Allergies { get; set; }
    }

    public class GenericNameSideEffects
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ICollection<GenericDrugName> GenericDrugName { get; set; }
    }

    public class GenericNameToxicites
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<GenericDrugName> GenericDrugName { get; set; }
    }

    public class GenericNameContraindication
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<GenericDrugName> GenericDrugName { get; set; }
    }

    public class GenericNameAllergies
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual ICollection<GenericDrugName> GenericDrugName { get; set; }
    }

    public  class Invoice
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
        public System.DateTime? TDateTime { get; set; }
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

        [ForeignKey("SupplierId")]
        public virtual supplier Supplier { get; set; }

        public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual ICollection<ProcurementInvoiceTransactions> ProcurementInvoiceTransactions { get; set; }
        


    }

    public  class InvoiceDetail
    {
        public int Id { get; set; }
        public int? InvoiceNo { get; set; }
        public string Item { get; set; }
        public string BatchNo { get; set; }
        public double Rate { get; set; }
        public int DrugId { get; set; }
        public int Units { get; set; }
        public int UnitsPerCase { get; set; }
        public int NoOfPack { get; set; }
        public double per { get; set; }
        public double CostPrice { get; set; }
        public double Amount { get; set; }
        public int FreeUnits { get; set; }
        public double CostPerCasePack { get; set; }
        public System.DateTime? MfgDate { get; set; }
        public string ExpiryStatus { get; set; }
        public string MfgCoNm { get; set; }
        public double MRP { get; set; }
        public double Discount { get; set; }
        public double VatAmt { get; set; }
        public string Vat_Cst { get; set; }
        public string VatCstOn { get; set; }
        public System.DateTime? ExpDate { get; set; }
        public string VAT_CstPercentage { get; set; }
        public double FreightCharges { get; set; }
        public double PackingCharges { get; set; }
        public double FinancialYearId { get; set; }
        public int FreeQty { get; set; }
        public double supp_vat { get; set; }
        public double CasePackRate { get; set; }
        public int UnitPack { get; set; }
        public string IssueToFlag { get; set; }
        public string StoreFlag { get; set; }
        public int companyid { get; set; }
        public int PrivQuantity { get; set; }
        public int ReceviedQty { get; set; }
        public System.DateTime? ReceviedDate { get; set; }
        public bool AckFlag { get; set; }
        public string Remark { get; set; }
        public string Discount_Flag { get; set; }
        public string FinalInvoiceNo { get; set; }
        public double MRPUnit { get; set; }
        public System.DateTime? tDateTime { get; set; }
        public string TaxTypeFlag { get; set; }
        public double Satvat { get; set; }
        public bool IsDonor { get; set; }

        

        [ForeignKey("InvoiceNo")]
        public virtual Invoice Invoice { get; set; }
    }

    public  class ItemMaster
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string MfgCoName { get; set; }
        public string GenericDrugName { get; set; }
        public string BatchNo { get; set; }
        public string Supplier { get; set; }
        public string Category { get; set; }
        public int CurrentStock { get; set; }
        public string MfgDate { get; set; }
        public int ReorderLevel { get; set; }
        public string ICDTenCode { get; set; }
        public string barCode { get; set; }
        public double CasePackRate { get; set; }
        public double CostPriceUnit { get; set; }
        public double SellingPriceUnit { get; set; }
        public string ExpiryStatus { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string PurchaseDate { get; set; }
        public string AssetStatus { get; set; }
        public string WarantyExpiryDate { get; set; }
        public string DateDisposed { get; set; }
        public string DateCommisioned { get; set; }
        public int UnitsPack { get; set; }
        public string StoreName { get; set; }
        public string RackName { get; set; }
        public int? CellInRack { get; set; }
        public int? DrugId { get; set; }

        //navigation properties
        public Drug Drug { get; set; }
    }

    public  class MfgCo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StoreName { get; set; }
    }

    public  class OrderDetail
    {
        [Key]
        public int OrderItemsID { get; set; }
        public int OrderID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }

        public virtual OrderMaster OrderMaster { get; set; }
    }

    public  class OrderMaster
    {
        [Key]
        public int OrderID { get; set; }
        public int OrderNo { get; set; }
        public System.DateTime OrderDate { get; set; }
        public string Description { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

    public  class Product
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
    }

    public  class Store
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
    }

    public  class supplier
    {
        [Key]
        public int Supplier_Code { get; set; }
        [Required]
        public string Supplier_Name { get; set; }
        [Required]
        public string Supplier_Address { get; set; }
        [Required]
        public string Supplier_City { get; set; }
        public string Supplier_TelNo { get; set; }
        public string Fax { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime? DateInserted { get; set; }
        public string Supplier_Type { get; set; }
        public string StoreName { get; set; }
    }

    public class Drug
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string IcdTenCode { get; set; }
        public int? GenericDrugNameId { get; set; }
        public int? CategoryId { get; set; }
        public int UnitsPack { get; set; }
        public int ReorderLevel { get; set; }
        public string RouteOfAdmin { get; set; }
        public string Remark { get; set; }
        public int? DoseId { get; set; }
        public string Rack { get; set; }
        public int? CellinRack { get; set; }
        public bool? IsVaccine { get; set; }
        public bool? IsStrip { get; set; }
        public bool? IsVitamin { get; set; }
        public string CreamSyrup { get; set; }
        public bool? IsActive { get; set; }

        //naviagtion fields
        public virtual GenericDrugName genericDrugName { get; set; }
        public virtual Category Category { get; set; }
        public virtual Dose Dose { get; set; }
    }

    public class Dose
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string StandardTime { get; set; }
        public string Description { get; set; }
    }

    public class ProvisionalPurchaseOrder
    {
        public int Id { get; set; }
        public string SupplierType { get; set; }
        public int PONumber { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public ICollection<supplier> Suppliers { get; set; }
        public ICollection<ProvisionalPOItemsDetail> POItemsDetail { get; set; }

    }

    public class ProvisionalPOItemsDetail
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public int Quantity { get; set; }
        public double CostPerUnit { get; set; }
        public double TotalCost { get; set; }
        public bool Accepted { get; set; }

        public virtual Drug Drug { get; set; }

    }

    /* This are the models in the medical store which sares same Db as the Procurement Module*/
    public class RackMaster
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int totalNumberOfCells { get; set; }
        public string department { get; set; }
    }

    //here is a simulation model for the patient transaction

    public class SimulationPatientIssueVoucher
    {
        public int Id { get; set; }
        public string Regno { get; set; }
        public string Opd { get; set; }
        public string Ipd { get; set; }
        public string HivNo { get; set; }
        public string PatientsName { get; set; }
        public int TransactionId { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public bool ItemIssued { get; set; }
        public DateTime? DateIssued { get; set; }
        public string VoucherNumber { get; set; }
        public int ClinicNo { get; set; }
        public double totalAmount { get; set; }
        public string DoctorsName { get; set; }
        public string Allergies { get; set; }
        public double PaidAmount { get; set; }
        public double Discount { get; set; }
        public bool isPaid { get; set; }
        public bool isFromDoctor { get; set; }
        public bool isPatiallyPaid { get; set; }
        public bool? IsWalkIn { get; set; }



        public ICollection<SimulationTreatment> SimulationTreatment { get; set; }

        //methods
        private void CalculateAmount()
        {
            if (SimulationTreatment != null)
            {
                foreach (var item in SimulationTreatment)
                {
                    totalAmount += item.Amount;
                }
            }
        }
    }


    public class SimulationTreatment
    {
        public int Id { get; set; }
        public int SimulationPatientIssueVoucherId { get; set; }
        public int ItemMasterId { get; set; }
        public int Rate { get; set; }
        public int units { get; set; }
        public double Amount { get; set; }
        public int NoOfDays { get; set; }
        public int Notes { get; set; }
        public string Description { get; set; }
        [DefaultValue(0)]
        public int ItemRefund { get; set; }
        [DefaultValue(false)]
        public bool Available { get; set; }
        public bool isPaid { get; set; }
        public bool isIssued { get; set; }
        public bool? DrugIsNotInHospitalDataBase { get; set; }
        public bool? IsWalkIn { get; set; }

        public SimulationPatientIssueVoucher SimulationPatientIssueVoucher { get; set; }
        public ItemMaster ItemMaster { get; set; }
    }

    public class DepartmentVoucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoucherNO { get; set; }
        public int DepartmentId { get; set; }
        public DateTime? IssueDate { get; set; }
        public decimal Total { get; set; }
        public bool IsItemIssued { get; set; }

        public ICollection<DepartmentVoucherItem> DepartmentVoucherItems { get; set; }

    }

    public class DepartmentVoucherItem
    {
        public int Id { get; set; }
        public int ItemMasterId { get; set; }

        [ForeignKey("DepartmentVoucher")]
        public int DepartmentVoucherId { get; set; }
        public int Units { get; set; }
        public decimal Amount { get; set; }


        public DepartmentVoucher DepartmentVoucher { get; set; }
        public ItemMaster ItemMaster { get; set; }

    }

    public class IssueForConsumption
    {
        public int Id { get; set; }
        public int ItemMasterId { get; set; }
        public int Units { get; set; }
        public decimal Amount { get; set; }
        public int Rate { get; set; }

        public ItemMaster ItemMaster { get; set; }
    }


    public class PhamarcyRequests
    {
        public int Id { get; set; }
        public string Issue { get; set; }
        public string RequestFrom { get; set; }
        public int WardNo { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestBy { get; set; }

        public ICollection<PharmacyRequestedItems> PharmacyRequestedItems { get; set; }

    }

    public class PharmacyRequestedItems
    {
        public int Id { get; set; }
        public int ItemMasterId { get; set; }
        public int RequiredQuantity { get; set; }
        public int PhamarcyRequestsId { get; set; }
        public string StoreName { get; set; }
        public bool? IsIssued { get; set; }
        public int CurrentStock { get; set; }
        public string ItemName { get; set; }

        //public ItemMaster ItemMaster { get; set; }
        public PhamarcyRequests PhamarcyRequests { get; set; }
    }

    public class RequestFromCSSD
    {
        public int Id { get; set; }
        public DateTime DateIssued { get; set; }
        public int RequestQuantity { get; set; }
        public string processed { get; set; }
    }

    public class DeptIndentList
    {
        public int Id { get; set; }
        public string  DeptName { get; set; }
        public string NurseStation { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public int ReqQty { get; set; }
        public string processed { get; set; }
    }

    public class ItemsRecieved
    {
        public int Id { get; set; }
        public string ReceivedItem { get; set; }
        public int InvoiceNumber { get; set; }
        public DateTime RecievedDate { get; set; }
        public string IssueTo { get; set; }
        public string IssueFrom { get; set; }
        public string processed { get; set; }

    }

    public class RecieveDetails
    {
        public int Id { get; set; }
        public int voucherNumber { get; set; }
        public DateTime RecievedDate { get; set; }
        public string IssueTo { get; set; }
        public string Detail { get; set; }
        public string processed { get; set; }
    }

    public class ProcurementInvoiceTransactions
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("FKInvoices")]
        public int InvoiceId { get; set; }

        public double TransactionAmountPaid { get; set; }
        public double PendingBalance { get; set; }

        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; }

        public virtual Invoice FKInvoices { get; set; }


    }

    public class MedicationRefund
    {
        public int Id { get; set; }
        public int ItemMasterId { get; set; }
        public int VoucherNo { get; set; }
        public int PatientId { get; set; }
        public int Rate { get; set; }
        public int RefundQty { get; set; }
        public DateTime RefundDate { get; set; }
        public double RefundAmount { get; set; }

        public ItemMaster ItemMaster { get; set; }

    }

    public class DrugTransactions
    {
        public int Id { get; set; }
        public int ItemMasterId { get; set; }
        public int QuantityIssued { get; set; }
        public int? DepartmentId { get; set; }
        public int? PatientId { get; set; }
        public int Rate { get; set; }
        public DateTime TransactionDate { get; set; }

        [DefaultValue(1)]
        public int User { get; set; }
        public bool? IsWalkIn { get; set; }
    }

    public class Walking
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public int Frequency { get; set; }
        public int Day { get; set; }
        public int Quantity { get; set; }
        public int RoutingAdmin { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public int PatientIdentifierId { get; set; }
        public int BranchId { get; set; }
        public DateTime TimeAdded { get; set; }
        public double UnitPrice { get; set; }
        public int QuantityIssued { get; set; }
        public double Subtotal { get; set; }
        public bool Paid { get; set; }
        public bool Issued { get; set; }
        public string PatientsName { get; set; }

        
    }
    public class DrugTariffs
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public int TariffId { get; set; }
        public string TarrifName { get; set; }
        public double DrugUnitPrice { get; set; }

        [DefaultValue(false)]
        public bool IsUnder5 { get; set; }

        public virtual Drug Drug { get; set; }
    }

    public class SearchPatientIssueVoucher
    {
        public int Id { get; set; }
        public int OpdNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool Paid { get; set; }
        public bool Available { get; set; }
        public int BillNo { get; set; }
        public bool IsFromDoctor { get; set; }
        public string PatientsName { get; set; }

    }

    public class StockAdjusted
    {
        public int Id { get; set; }
        public DateTime DateAdjusted { get; set; }
        public int ItemMasterId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public string Department { get; set; }


        public ItemMaster ItemMaster { get; set; }
    }

}