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
    
    public partial class FallRisk
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public string PatientType { get; set; }
        public string RegNo { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Nationality { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string AgeParameter { get; set; }
        public string AgeScoreTime { get; set; }
        public string AgeComment { get; set; }
        public string GenderParameter { get; set; }
        public string GenderScoreTime { get; set; }
        public string GenderComment { get; set; }
        public string DiagnosisParameter { get; set; }
        public string DiagnosisScoreTime { get; set; }
        public string DiagnosisComment { get; set; }
        public string CognitiveParameter { get; set; }
        public string CognitiveScoreTime { get; set; }
        public string CognitiveComment { get; set; }
        public string EnvironmentalParameter { get; set; }
        public string EnvironmentalScoreTime { get; set; }
        public string EnvironmentalComment { get; set; }
        public string ResponseParameter { get; set; }
        public string ResponseScoreTime { get; set; }
        public string ResponseComment { get; set; }
        public string MedicationParameter { get; set; }
        public string MedicationScoreTime { get; set; }
        public string MedicationComment { get; set; }
        public Nullable<System.DateTime> DatePedetriac { get; set; }
        public string FallingParameter { get; set; }
        public string FallingScoreTime { get; set; }
        public string FallingComment { get; set; }
        public string SecondaryParameter { get; set; }
        public string SecondaryScoreTime { get; set; }
        public string SecondaryComment { get; set; }
        public string Ambulatory1Parameter { get; set; }
        public string Ambulatory2Parameter { get; set; }
        public string Ambulatory3Parameter { get; set; }
        public string Ambulatory1ScoreTime { get; set; }
        public string Ambulatory2ScoreTime { get; set; }
        public string Ambulatory3ScoreTime { get; set; }
        public string AmbulatoryComment { get; set; }
        public string IntravenousParameter { get; set; }
        public string IntravenousScoreTime { get; set; }
        public string IntravenousComment { get; set; }
        public string GaitParameter { get; set; }
        public string GaitScoreTime { get; set; }
        public string WeakScoreTime { get; set; }
        public string ImpairedScoreTime { get; set; }
        public string GaitComment { get; set; }
        public string MentalParameter { get; set; }
        public string Mental1ScoreTime { get; set; }
        public string Mental2ScoreTime { get; set; }
        public string MentalComment { get; set; }
        public Nullable<System.DateTime> DateAdult { get; set; }
    }
}
