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
    
    public partial class NutritionChildRegister
    {
        public int id { get; set; }
        public Nullable<int> OPDID { get; set; }
        public string ServicePoint { get; set; }
        public Nullable<System.DateTime> VisitDate { get; set; }
        public string VisitType { get; set; }
        public Nullable<decimal> PatientNo { get; set; }
        public string PatientName { get; set; }
        public Nullable<decimal> age { get; set; }
        public string gender { get; set; }
        public string LinkedOVC { get; set; }
        public string PatientCategory { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public Nullable<decimal> HeightforAge { get; set; }
        public string edema { get; set; }
        public Nullable<decimal> WeightForHeight { get; set; }
        public Nullable<decimal> HeightLength { get; set; }
        public Nullable<decimal> bmi { get; set; }
        public Nullable<decimal> muac { get; set; }
        public Nullable<decimal> bmiz { get; set; }
        public string NutritionStatus { get; set; }
        public string Serostatus { get; set; }
        public string SamMamClients { get; set; }
        public string OnARVs { get; set; }
        public string CoexistingConditions { get; set; }
        public string Anaemia { get; set; }
        public Nullable<decimal> MetabolicDisorders { get; set; }
        public Nullable<decimal> AllergiesIntolerance { get; set; }
        public string CD4Count { get; set; }
        public string FoodSecure { get; set; }
        public string First0to6Months { get; set; }
        public string Sixto12Months { get; set; }
        public string maternalNutrition { get; set; }
        public string CriticalNutritionPractices { get; set; }
        public string TherapeuticFoods { get; set; }
        public string SupplementalFoods { get; set; }
        public Nullable<decimal> Micronutrients { get; set; }
        public string OutcomeClient { get; set; }
        public string ReferralsandTransfers { get; set; }
        public Nullable<System.DateTime> TCADate { get; set; }
        public string Designation { get; set; }
        public string Initials { get; set; }
        public Nullable<System.DateTime> date_time { get; set; }
    }
}
