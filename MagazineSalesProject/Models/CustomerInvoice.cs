namespace MagazineSalesProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;

    public class CustomerInvoice
    {
        public CustomerInvoice()
        {

        }

        [Key]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int SubscriptionsBought { get; set; }
        public int InvoiceNumber { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string CardNumber { get; set; }
        public Nullable<int> ExpirationMonth { get; set; }
        public Nullable<int> ExpirationYear { get; set; }
        public int SellerID { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
    }
}