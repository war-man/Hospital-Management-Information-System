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
    
    public partial class JPayAutoPayment
    {
        public int Id { get; set; }
        public Nullable<int> OPDNo { get; set; }
        public int JPayId { get; set; }
        public double PaidAmount { get; set; }
        public string BilledServices { get; set; }
        public string BilledMedication { get; set; }
        public string BilledWalkinMeds { get; set; }
        public System.DateTime DateAdded { get; set; }
    
        public virtual JPayment JPayment { get; set; }
        public virtual OpdRegister OpdRegister { get; set; }
    }
}
