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
    
    public partial class WardTypeCharge
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int WardCategoryId { get; set; }
        public double Price { get; set; }
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual HSWardCategory HSWardCategory { get; set; }
        public virtual Service Service { get; set; }
    }
}
