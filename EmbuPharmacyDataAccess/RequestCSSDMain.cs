//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmbuPharmacyDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class RequestCSSDMain
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RequestCSSDMain()
        {
            this.RequestCSSDDetails = new HashSet<RequestCSSDDetail>();
        }
    
        public int ReqId { get; set; }
        public string StoreFlag { get; set; }
        public string IssuetoFlag { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> TDatetime { get; set; }
        public string ReqType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestCSSDDetail> RequestCSSDDetails { get; set; }
    }
}
