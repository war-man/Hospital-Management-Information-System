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
    
    public partial class Questionnaire
    {
        public int Id { get; set; }
        public int OPDno { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string CameWith1 { get; set; }
        public string CameWith2 { get; set; }
        public string CameWith3 { get; set; }
        public string CameWith4 { get; set; }
        public string Fear1 { get; set; }
        public string Fear2 { get; set; }
        public string Fear3 { get; set; }
        public string Fear4 { get; set; }
        public string Reason1 { get; set; }
        public string Reason2 { get; set; }
        public string Reason3 { get; set; }
        public string Reason4 { get; set; }
        public string ExamInformed { get; set; }
        public string InformationSatisfication { get; set; }
        public string ExamSatisfication { get; set; }
        public string CryoProcedure { get; set; }
        public string AllQuestionedAnswered { get; set; }
        public string ComplicationInformed { get; set; }
        public string CryotherapySatisfication { get; set; }
        public string HusbandAbstain { get; set; }
        public string Privacy { get; set; }
        public string LikeCost { get; set; }
        public string LikeTherapist { get; set; }
        public string LikeServices { get; set; }
        public string LikeActivity { get; set; }
        public string DislikeCost { get; set; }
        public string DislikeTherapist { get; set; }
        public string DislikeServices { get; set; }
        public string DislikeActivity { get; set; }
        public string SuggestionCost { get; set; }
        public string SuggestionTherapist { get; set; }
        public string SuggestionServices { get; set; }
        public string SuggestionActivity { get; set; }
        public string Recommend { get; set; }
        public string ComeBack { get; set; }
        public string ScreeningLocation { get; set; }
        public string CashAffordable { get; set; }
    
        public virtual OpdRegister OpdRegister { get; set; }
    }
}
