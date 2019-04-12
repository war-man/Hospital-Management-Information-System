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
    
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.LeaveLetters = new HashSet<LeaveLetter>();
            this.Users = new HashSet<User>();
        }
    
        public int Id { get; set; }
        public string RollNo { get; set; }
        public string Salutation { get; set; }
        public string FName { get; set; }
        public string OtherName { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string IdNo { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Nullable<int> DesignationId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public int DepartmentId { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
    
        public virtual Branch Branch { get; set; }
        public virtual Department Department { get; set; }
        public virtual Designation Designation { get; set; }
        public virtual Designation Designation1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaveLetter> LeaveLetters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
