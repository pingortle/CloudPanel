//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CloudPanel.Modules.Database.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Domain
    {
        public int DomainID { get; set; }
        public string CompanyCode { get; set; }
        public string Domain1 { get; set; }
        public Nullable<bool> IsSubDomain { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAcceptedDomain { get; set; }
        public Nullable<bool> IsLyncDomain { get; set; }
    }
}
