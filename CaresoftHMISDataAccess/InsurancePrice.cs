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
    
    public partial class InsurancePrice
    {
        public int Id { get; set; }
        public int ServicePriceId { get; set; }
        public int CompanyId { get; set; }
        public double Price { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual ServicesPrice ServicesPrice { get; set; }
    }
}
