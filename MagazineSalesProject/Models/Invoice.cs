//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MagazineSalesProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Invoice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Invoice()
        {
            this.InvoiceContains = new HashSet<InvoiceContain>();
        }
    
        public int InvoiceNumber { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string CardNumber { get; set; }
        public Nullable<int> ExpirationMonth { get; set; }
        public Nullable<int> ExpirationYear { get; set; }
        public int SellerID { get; set; }
        public string Email { get; set; }
        public System.DateTime OrderDate { get; set; }
    
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceContain> InvoiceContains { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
