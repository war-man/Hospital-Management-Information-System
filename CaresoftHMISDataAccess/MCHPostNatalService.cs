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
    
    public partial class MCHPostNatalService
    {
        public int Id { get; set; }
        public int OPDNo { get; set; }
        public int BranchId { get; set; }
        public int UserId { get; set; }
        public System.DateTime DateAdded { get; set; }
        public string DeliveryPlace { get; set; }
        public Nullable<System.DateTime> AdmissionDate { get; set; }
        public Nullable<System.DateTime> DischargeDate { get; set; }
        public string AdmissionOutcome { get; set; }
        public string FirstStageDuration { get; set; }
        public string SecondStageDuration { get; set; }
        public string ThirdStageDuration { get; set; }
        public string TotalLabourTime { get; set; }
        public string DeliveryType { get; set; }
        public string WithMidwife { get; set; }
        public string WithTBA { get; set; }
        public string DeliveryParity { get; set; }
        public string BirthType { get; set; }
        public string WithPhysician { get; set; }
        public string OtherAssistance { get; set; }
        public string OtherDeliverySpecification { get; set; }
        public string MotherCondition { get; set; }
        public Nullable<int> Temperature { get; set; }
        public string Pulse { get; set; }
        public string Respiratory { get; set; }
        public string Fundus { get; set; }
        public string PVLoss { get; set; }
        public string BP { get; set; }
        public string DeliveryComplication { get; set; }
        public string BabySex { get; set; }
        public Nullable<int> BabyWeight { get; set; }
        public Nullable<int> OneMinScore { get; set; }
        public Nullable<int> FiveMininScore { get; set; }
        public Nullable<int> TenMinScore { get; set; }
        public string Abnomalities { get; set; }
        public string BabyStatus { get; set; }
        public string Pathology { get; set; }
        public string PathologyDetails { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
    }
}
