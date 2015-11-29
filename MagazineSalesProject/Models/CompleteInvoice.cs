using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MagazineSalesProject.Models
{
    public class CompleteInvoice
    {
        public Invoice inv;
        public List<Magazine> magList = new List<Magazine>();
    }
}