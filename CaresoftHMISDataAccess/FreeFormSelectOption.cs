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
    
    public partial class FreeFormSelectOption
    {
        public int Id { get; set; }
        public int MarkupId { get; set; }
        public string SelectOption { get; set; }
        public string OptionValue { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime TimeAdded { get; set; }
    
        public virtual FreeFormMarkup FreeFormMarkup { get; set; }
    }
}
