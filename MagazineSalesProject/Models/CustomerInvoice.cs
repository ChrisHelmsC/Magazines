namespace MagazineSalesProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ComponentModel.DataAnnotations;

    public class CustomerInvoice
    {

        public Customer cust;
        public List<Invoice> invoices = new List<Invoice>();
    }
}