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
    
    public partial class LasikClinicEntry
    {
        public int Id { get; set; }
        public Nullable<int> BranchId { get; set; }
        public int UserId { get; set; }
        public System.DateTime DateAdded { get; set; }
        public int OPDNo { get; set; }
        public string SpectaclesUsage { get; set; }
        public string SpectacleDuration { get; set; }
        public Nullable<System.DateTime> LastCheckUp { get; set; }
        public string ContactLenseUsage { get; set; }
        public string ContactLenseDuration { get; set; }
        public string ContactLenseType { get; set; }
        public string RightVAUnaided { get; set; }
        public string LeftVAUnaided { get; set; }
        public string RightVAAided { get; set; }
        public string LeftVAAided { get; set; }
        public string RightIOP { get; set; }
        public string LeftIOP { get; set; }
        public string RightCorneaIrregularity { get; set; }
        public string LeftCorneaIrregularity { get; set; }
        public string RightShirmer { get; set; }
        public string LeftShirmer { get; set; }
        public string RightPupilsize { get; set; }
        public string LeftPupilsize { get; set; }
        public string RightRefraction { get; set; }
        public string LeftRefraction { get; set; }
        public string RightPMT { get; set; }
        public string LeftPMT { get; set; }
        public string RightFundusCentral { get; set; }
        public string LeftFundusCentral { get; set; }
        public string RightFundusPeriphery { get; set; }
        public string LeftFundusPeriphery { get; set; }
        public string RightKReading { get; set; }
        public string LeftKReading { get; set; }
        public string RightPachemetry { get; set; }
        public string LeftPachemetry { get; set; }
        public string RightPotography { get; set; }
        public string LeftPotography { get; set; }
        public string InformedConcent { get; set; }
        public string OpticZone { get; set; }
        public string Ablation { get; set; }
        public string Specification { get; set; }
        public string LeftCorneaOpacity { get; set; }
        public string RightCorneaOpacity { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
    }
}
