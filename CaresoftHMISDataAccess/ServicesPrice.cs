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
    
    public partial class ServicesPrice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServicesPrice()
        {
            this.InsurancePrices = new HashSet<InsurancePrice>();
        }
    
        public int Id { get; set; }
        public int TariffId { get; set; }
        public int ServiceId { get; set; }
        public double Award { get; set; }
        public string AwardUnit { get; set; }
        public double DoctorFee { get; set; }
        public string DoctorFeeUnit { get; set; }
        public Nullable<int> WardCategoryId { get; set; }
        public int BranchId { get; set; }
        public int UserId { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InsurancePrice> InsurancePrices { get; set; }
        public virtual Service Service { get; set; }
        public virtual Tariff Tariff { get; set; }
    }
}
