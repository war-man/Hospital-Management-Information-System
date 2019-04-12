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
    
    public partial class GlucomaPerfomaEntry
    {
        public int Id { get; set; }
        public Nullable<int> BranchId { get; set; }
        public System.DateTime DateAdded { get; set; }
        public int OPDNo { get; set; }
        public int UserId { get; set; }
        public Nullable<int> IllnessDuration { get; set; }
        public string DrugsTreatment { get; set; }
        public string LaserTreatment { get; set; }
        public string SurgeryTreatment { get; set; }
        public string FamilyHiistory { get; set; }
        public string HypertensionIllness { get; set; }
        public string DMIllness { get; set; }
        public string CADIllness { get; set; }
        public string OtherIllnesses { get; set; }
        public string RightPriorTreatment { get; set; }
        public string LeftPriorTreatment { get; set; }
        public string RightWithTreatment { get; set; }
        public string LeftWithTreatment { get; set; }
        public string RightDiurnal { get; set; }
        public string LeftDiurnal { get; set; }
        public string RightVisualAcuity { get; set; }
        public string LeftVisualAcuity { get; set; }
        public string RightRefraction { get; set; }
        public string LeftRefraction { get; set; }
        public string RightAntSegment { get; set; }
        public string LeftAntSegment { get; set; }
        public string RightNviNva { get; set; }
        public string LeftNviNva { get; set; }
        public string RightONHCDRatio { get; set; }
        public string LeftONHCDRatio { get; set; }
        public string RightNrrColour { get; set; }
        public string LeftNrrColour { get; set; }
        public string RightNfl { get; set; }
        public string LeftNfl { get; set; }
        public string RightGonioscopy { get; set; }
        public string LeftGonioscopy { get; set; }
        public string RightCorneaThickness { get; set; }
        public string LeftCorneaThickness { get; set; }
        public string RightMd { get; set; }
        public string LeftMd { get; set; }
        public string RightPsd { get; set; }
        public string LeftPsd { get; set; }
        public string RightCpsd { get; set; }
        public string LeftCpsd { get; set; }
        public string RightProgression { get; set; }
        public string LeftProgression { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
    }
}
