using Caresoft2._0.Areas.GeneralStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Procurement.Models
{
    public class ProcurementDbContext : DbContext
    {
        public ProcurementDbContext(): base("ProcurementEntities")
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<GenericDrugName> GenericDrugName { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetail { get; set; }
        public DbSet<ItemMaster> ItemMaster { get; set; }
        public DbSet<MfgCo> MfgCo { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderMaster> OrderMaster { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Store> Store { get; set; }
        public DbSet<supplier> supplier { get; set; }
        public DbSet<Drug> Drug { get; set; }
        public DbSet<ProvisionalPurchaseOrder> ProvisionalPurchaseOrder { get; set; }
        public DbSet<ProvisionalPOItemsDetail> ProvisionalPOItemsDetail { get; set; }
        
       

        //Departments Medical Store, the model classes for this are togther with procurement
        public DbSet<DepartmentVoucher> DepartmentVoucher { get; set; }
        public DbSet<DepartmentVoucherItem> DepartmentVoucherItem { get; set; }
        public DbSet<IssueForConsumption> IssueForConsumption { get; set; }
        public DbSet<PhamarcyRequests> PhamarcyRequests { get; set; }
        public DbSet<RequestFromCSSD> RequestFromCSSD { get; set; }
        public DbSet<DeptIndentList> DeptIndentList { get; set; }
        public DbSet<ItemsRecieved> ItemsRecieved { get; set; }
        public DbSet<RecieveDetails> RecieveDetails { get; set; }
        public DbSet<Dose> Dose { get; set; }
        public DbSet<RackMaster> RackMaster { get; set; }
        public DbSet<GenericNameAllergies> GenericNameAllergies { get; set; }
        public DbSet<GenericNameContraindication> GenericNameContraindication { get; set; }
        public DbSet<GenericNameSideEffects> GenericNameSideEffects { get; set; }
        public DbSet<GenericNameToxicites> GenericNameToxicites { get; set; }

        //simulation data for treatment
        public DbSet<SimulationPatientIssueVoucher> SimulationPatientIssueVoucher { get; set; }
        public DbSet<SimulationTreatment> SimulationTreatment { get; set; }


        //GENERAL STORE
        public DbSet<ItemMasterGstore> ItemMasterGstore { get; set; }
        public DbSet<CategoryGstore> CategoryGstore { get; set; }
        public DbSet<PharmacyRequestedItems> PharmacyRequestedItems { get; set; }

        //Procurement 
        public DbSet<ProcurementInvoiceTransactions> ProcurementInvoiceTransactions { get; set; }


        //Transaction Table contains all the drug transactions ever done in a systems
        public DbSet<DrugTransactions> DrugTransactions { get; set; }
        public DbSet<DrugTariffs> DrugTariffs { get; set; }


        public DbSet<Walking> Walkings { get; set; }

        public DbSet<MedicationRefund> MedicationRefund { get; set; }

        public DbSet<SearchPatientIssueVoucher> searchPatientIssueVouchers { get; set; }

        public DbSet<StockAdjusted> StockAdjusted { get; set; }


    }
}