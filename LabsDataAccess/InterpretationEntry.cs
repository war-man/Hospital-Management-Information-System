//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LabsDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class InterpretationEntry
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public Nullable<int> Test { get; set; }
        public Nullable<int> Profiles { get; set; }
        public string Data { get; set; }
        public Nullable<System.DateTime> CreatedUtc { get; set; }
        public int DepartmentRadPath { get; set; }
        public int BranchId { get; set; }
    
        public virtual LabTest LabTest { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
