using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MagazineSalesProject.Models
{
    
    public class EmailSeller
    {
        [Key]
        public string Email { get; set; }
        public int SellerID { get; set; }
        public List<Magazine> magList { get; set; }
        public decimal total = 0;
        public string magToAdd = "";

        public List<Magazine> magazineList { get; set; }
    }
}
