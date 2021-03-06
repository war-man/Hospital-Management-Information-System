﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EmbuCounty_PharmacyEntities : DbContext
    {
        public EmbuCounty_PharmacyEntities()
            : base("name=EmbuCounty_PharmacyEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AcceptedProvisionalPurchaseOrder> AcceptedProvisionalPurchaseOrders { get; set; }
        public virtual DbSet<AcceptedProvisionalPurchaseOrderItemsDetail> AcceptedProvisionalPurchaseOrderItemsDetails { get; set; }
        public virtual DbSet<AMCMaster> AMCMasters { get; set; }
        public virtual DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public virtual DbSet<AreaMaster> AreaMasters { get; set; }
        public virtual DbSet<AssignRoomToNurStationDetail> AssignRoomToNurStationDetails { get; set; }
        public virtual DbSet<AuditPharmacy> AuditPharmacies { get; set; }
        public virtual DbSet<BillTransaction> BillTransactions { get; set; }
        public virtual DbSet<BillTransaction_LF> BillTransaction_LF { get; set; }
        public virtual DbSet<BillTransDetail> BillTransDetails { get; set; }
        public virtual DbSet<BillTransDetail_LF> BillTransDetail_LF { get; set; }
        public virtual DbSet<BillTransInfo> BillTransInfoes { get; set; }
        public virtual DbSet<Bom_Details> Bom_Details { get; set; }
        public virtual DbSet<Bom_Main> Bom_Main { get; set; }
        public virtual DbSet<BuildingMaster> BuildingMasters { get; set; }
        public virtual DbSet<CleanupRequest> CleanupRequests { get; set; }
        public virtual DbSet<CleanupRequestTransaction> CleanupRequestTransactions { get; set; }
        public virtual DbSet<CleanUpSchedule> CleanUpSchedules { get; set; }
        public virtual DbSet<CleanUpTransaction> CleanUpTransactions { get; set; }
        public virtual DbSet<ComplaintMaster> ComplaintMasters { get; set; }
        public virtual DbSet<DrugExchanage> DrugExchanages { get; set; }
        public virtual DbSet<DrugMaster> DrugMasters { get; set; }
        public virtual DbSet<DrugMaster_old> DrugMaster_old { get; set; }
        public virtual DbSet<DrugOfferDetail> DrugOfferDetails { get; set; }
        public virtual DbSet<FloorAreaMaster> FloorAreaMasters { get; set; }
        public virtual DbSet<FloorMaster> FloorMasters { get; set; }
        public virtual DbSet<GenericLabDetail> GenericLabDetails { get; set; }
        public virtual DbSet<GroupMst> GroupMsts { get; set; }
        public virtual DbSet<GroupMst_Company> GroupMst_Company { get; set; }
        public virtual DbSet<GroupMst_old> GroupMst_old { get; set; }
        public virtual DbSet<HouseKeeperMaster> HouseKeeperMasters { get; set; }
        public virtual DbSet<Indent> Indents { get; set; }
        public virtual DbSet<IndentDetail> IndentDetails { get; set; }
        public virtual DbSet<Installationmaster> Installationmasters { get; set; }
        public virtual DbSet<InstructionMaster> InstructionMasters { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual DbSet<InvoiceNumber> InvoiceNumbers { get; set; }
        public virtual DbSet<InvoiceTransaction> InvoiceTransactions { get; set; }
        public virtual DbSet<InvoiceTransDetail> InvoiceTransDetails { get; set; }
        public virtual DbSet<IssueToLoundry> IssueToLoundries { get; set; }
        public virtual DbSet<Item_Transaction> Item_Transaction { get; set; }
        public virtual DbSet<ItemMaster> ItemMasters { get; set; }
        public virtual DbSet<ItemMaster_Company> ItemMaster_Company { get; set; }
        public virtual DbSet<ItemMaster_CSSD> ItemMaster_CSSD { get; set; }
        public virtual DbSet<ItemMaster_M> ItemMaster_M { get; set; }
        public virtual DbSet<ItemMaster_old> ItemMaster_old { get; set; }
        public virtual DbSet<ItemMaster_old2> ItemMaster_old2 { get; set; }
        public virtual DbSet<ItemMaster_W> ItemMaster_W { get; set; }
        public virtual DbSet<ItemNomaster> ItemNomasters { get; set; }
        public virtual DbSet<ItemStockLedger> ItemStockLedgers { get; set; }
        public virtual DbSet<Labconsumption> Labconsumptions { get; set; }
        public virtual DbSet<LabDetailsAgainstGenericDrug> LabDetailsAgainstGenericDrugs { get; set; }
        public virtual DbSet<LocationMaster> LocationMasters { get; set; }
        public virtual DbSet<LoundryMaster> LoundryMasters { get; set; }
        public virtual DbSet<LoundryReceivedFromWard> LoundryReceivedFromWards { get; set; }
        public virtual DbSet<Manufacturecompanymaster> Manufacturecompanymasters { get; set; }
        public virtual DbSet<MessageMaster> MessageMasters { get; set; }
        public virtual DbSet<NurseReturnLinenToHk> NurseReturnLinenToHks { get; set; }
        public virtual DbSet<NurseStationMaster> NurseStationMasters { get; set; }
        public virtual DbSet<Operation_Schedule> Operation_Schedule { get; set; }
        public virtual DbSet<OT_Schedule> OT_Schedule { get; set; }
        public virtual DbSet<PatientInfo> PatientInfoes { get; set; }
        public virtual DbSet<PMAsssignTo> PMAsssignToes { get; set; }
        public virtual DbSet<PMMaterial> PMMaterials { get; set; }
        public virtual DbSet<PMSchedule> PMSchedules { get; set; }
        public virtual DbSet<PMTaskSafetyInstruction> PMTaskSafetyInstructions { get; set; }
        public virtual DbSet<PreventiveMiantenance> PreventiveMiantenances { get; set; }
        public virtual DbSet<ProvisionalPurchaseOrder> ProvisionalPurchaseOrders { get; set; }
        public virtual DbSet<ProvisionalPurchaseOrderItemsDetail> ProvisionalPurchaseOrderItemsDetails { get; set; }
        public virtual DbSet<ProvisionalPurchaseOrderSupplierDetail> ProvisionalPurchaseOrderSupplierDetails { get; set; }
        public virtual DbSet<RateType> RateTypes { get; set; }
        public virtual DbSet<RateType_ForAwardAmount> RateType_ForAwardAmount { get; set; }
        public virtual DbSet<Refund_Details> Refund_Details { get; set; }
        public virtual DbSet<Refund_Main> Refund_Main { get; set; }
        public virtual DbSet<RefundReqFromNurse> RefundReqFromNurses { get; set; }
        public virtual DbSet<Reimbursement> Reimbursements { get; set; }
        public virtual DbSet<ReimbursementDetail> ReimbursementDetails { get; set; }
        public virtual DbSet<Report_setting> Report_setting { get; set; }
        public virtual DbSet<RequestCSSDDetail> RequestCSSDDetails { get; set; }
        public virtual DbSet<RequestCSSDMain> RequestCSSDMains { get; set; }
        public virtual DbSet<RequestDeptDetail> RequestDeptDetails { get; set; }
        public virtual DbSet<RequestDeptMain> RequestDeptMains { get; set; }
        public virtual DbSet<RequestForInventory> RequestForInventories { get; set; }
        public virtual DbSet<RequestForInventoryDetail> RequestForInventoryDetails { get; set; }
        public virtual DbSet<RequestForNewItem> RequestForNewItems { get; set; }
        public virtual DbSet<RequestTransDetail> RequestTransDetails { get; set; }
        public virtual DbSet<ReturnIssueVoucher> ReturnIssueVouchers { get; set; }
        public virtual DbSet<RoomMaster> RoomMasters { get; set; }
        public virtual DbSet<ScheduleMaster> ScheduleMasters { get; set; }
        public virtual DbSet<ServiceMaster> ServiceMasters { get; set; }
        public virtual DbSet<SetDrugLocation> SetDrugLocations { get; set; }
        public virtual DbSet<ShiftMaster> ShiftMasters { get; set; }
        public virtual DbSet<StockTaking> StockTakings { get; set; }
        public virtual DbSet<SupplierMaster> SupplierMasters { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tb_CompThemes> tb_CompThemes { get; set; }
        public virtual DbSet<tb_GenericDrug> tb_GenericDrug { get; set; }
        public virtual DbSet<tb_GenericDrug_old> tb_GenericDrug_old { get; set; }
        public virtual DbSet<tb_GenericDrug_old2> tb_GenericDrug_old2 { get; set; }
        public virtual DbSet<tb_MedicineRequirement> tb_MedicineRequirement { get; set; }
        public virtual DbSet<tb_MedicineRequirementMain> tb_MedicineRequirementMain { get; set; }
        public virtual DbSet<tb_Themes> tb_Themes { get; set; }
        public virtual DbSet<Tbl_AcceptedQty> Tbl_AcceptedQty { get; set; }
        public virtual DbSet<Tbl_CellMaster> Tbl_CellMaster { get; set; }
        public virtual DbSet<Tbl_ChamberMaster> Tbl_ChamberMaster { get; set; }
        public virtual DbSet<Tbl_ChamberMasterDetail> Tbl_ChamberMasterDetail { get; set; }
        public virtual DbSet<Tbl_ColdChainEquipment> Tbl_ColdChainEquipment { get; set; }
        public virtual DbSet<Tbl_DrugEffectMaster> Tbl_DrugEffectMaster { get; set; }
        public virtual DbSet<Tbl_Error> Tbl_Error { get; set; }
        public virtual DbSet<Tbl_GRNReturn> Tbl_GRNReturn { get; set; }
        public virtual DbSet<Tbl_IssueItem> Tbl_IssueItem { get; set; }
        public virtual DbSet<Tbl_IssueItemToPatient> Tbl_IssueItemToPatient { get; set; }
        public virtual DbSet<Tbl_IssueSetToPatient> Tbl_IssueSetToPatient { get; set; }
        public virtual DbSet<Tbl_ItemMasterLF> Tbl_ItemMasterLF { get; set; }
        public virtual DbSet<Tbl_ItemSendToCSSD> Tbl_ItemSendToCSSD { get; set; }
        public virtual DbSet<Tbl_MissingItem> Tbl_MissingItem { get; set; }
        public virtual DbSet<Tbl_MissingSet> Tbl_MissingSet { get; set; }
        public virtual DbSet<Tbl_PackageDescription> Tbl_PackageDescription { get; set; }
        public virtual DbSet<Tbl_PackageDetail> Tbl_PackageDetail { get; set; }
        public virtual DbSet<Tbl_PackageMaster> Tbl_PackageMaster { get; set; }
        public virtual DbSet<Tbl_Prescription> Tbl_Prescription { get; set; }
        public virtual DbSet<Tbl_ReqForItem> Tbl_ReqForItem { get; set; }
        public virtual DbSet<Tbl_RequestForPack> Tbl_RequestForPack { get; set; }
        public virtual DbSet<Tbl_RouteMaster> Tbl_RouteMaster { get; set; }
        public virtual DbSet<Tbl_RowMaster> Tbl_RowMaster { get; set; }
        public virtual DbSet<Tbl_ShelfDetail> Tbl_ShelfDetail { get; set; }
        public virtual DbSet<Tbl_ShelfMaster> Tbl_ShelfMaster { get; set; }
        public virtual DbSet<Tbl_SterileItem> Tbl_SterileItem { get; set; }
        public virtual DbSet<Tbl_SubPack> Tbl_SubPack { get; set; }
        public virtual DbSet<tbl_SupplierOffer> tbl_SupplierOffer { get; set; }
        public virtual DbSet<tbl_Supplierratelist> tbl_Supplierratelist { get; set; }
        public virtual DbSet<tbl_Supplierratelist_Tran> tbl_Supplierratelist_Tran { get; set; }
        public virtual DbSet<Tbl_VaccinConsumption> Tbl_VaccinConsumption { get; set; }
        public virtual DbSet<Temp_ItemStockLedger> Temp_ItemStockLedger { get; set; }
        public virtual DbSet<TempDeptIndentNurse> TempDeptIndentNurses { get; set; }
        public virtual DbSet<TempMedicalIndent> TempMedicalIndents { get; set; }
        public virtual DbSet<TmpGetLabconsumption> TmpGetLabconsumptions { get; set; }
        public virtual DbSet<TmpGetTreatment> TmpGetTreatments { get; set; }
        public virtual DbSet<TransactionInfo> TransactionInfoes { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<VatMaster> VatMasters { get; set; }
        public virtual DbSet<WardMaster> WardMasters { get; set; }
        public virtual DbSet<WorkNameMaster> WorkNameMasters { get; set; }
        public virtual DbSet<WorkTradeMaster> WorkTradeMasters { get; set; }
        public virtual DbSet<WorkTypeMaster> WorkTypeMasters { get; set; }
        public virtual DbSet<BillTransactionold> BillTransactionolds { get; set; }
        public virtual DbSet<DrugTypeMaster> DrugTypeMasters { get; set; }
        public virtual DbSet<FinancialYear> FinancialYears { get; set; }
        public virtual DbSet<FP_MethodsMaster> FP_MethodsMaster { get; set; }
        public virtual DbSet<Returnable_Items> Returnable_Items { get; set; }
        public virtual DbSet<ReturnIssueVoucherDetail> ReturnIssueVoucherDetails { get; set; }
        public virtual DbSet<TB_StatusMaster> TB_StatusMaster { get; set; }
        public virtual DbSet<Tbl_BarCodePrint> Tbl_BarCodePrint { get; set; }
        public virtual DbSet<Tbl_NurseIndentTemp> Tbl_NurseIndentTemp { get; set; }
        public virtual DbSet<TempTable2> TempTable2 { get; set; }
        public virtual DbSet<TempTable3> TempTable3 { get; set; }
        public virtual DbSet<TempTransaction> TempTransactions { get; set; }
        public virtual DbSet<Tmp_VaccineReport> Tmp_VaccineReport { get; set; }
        public virtual DbSet<Tmp_VaccRecIss> Tmp_VaccRecIss { get; set; }
    }
}
