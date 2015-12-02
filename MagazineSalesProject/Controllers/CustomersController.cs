using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MagazineSalesProject.Models;

namespace MagazineSalesProject.Controllers
{
    public class CustomersController : Controller
    {
        private MagazineDataEntities db = new MagazineDataEntities();
        private static EmailSeller ES;
        private static int sellerID = -1;

        public ActionResult AddMagazine()
        {
            String mName = Convert.ToString(Request["name"].ToString());
            String mDescription = Convert.ToString(Request["description"].ToString());
            String mGenre = Convert.ToString(Request["genre"].ToString());
            decimal mPrice = Convert.ToDecimal(Request["price"].ToString());
            String mPublisher =  Convert.ToString(Request["publisher"].ToString());

            Magazine magazine = new Magazine {Name = mName, Description = mDescription, Genre = mGenre, Price = mPrice, Publisher = mPublisher};

            var pubList = from n in db.Publishers
                          where n.Name == magazine.Publisher
                          select n;

            var genreList = from m in db.Genres
                            where m.Name == magazine.Genre
                            select m;

            if(!genreList.Any())
            {
                Genre gen = new Genre { Name = magazine.Genre };
                db.Genres.Add(gen);
                db.SaveChanges();
            }

            if (!pubList.Any())
            {
                Publisher pub = new Publisher { Name = magazine.Publisher };
                db.Publishers.Add(pub);
                db.SaveChanges();
            }

            db.Magazines.Add(magazine);
            db.SaveChanges();

            return View("~/Views/Magazines/Index.cshtml", null);
        }

        public ActionResult Logout()
        {
            sellerID = -1;
            return View("~/Views/Home/Login.cshtml" ,null);
        }

        public ActionResult IndexMagazine()
        {
            var clear = from n in db.Sellers
                        where n.SellerID == sellerID
                        select n;

            if (clear.First().clearance == 0)
            {
                return View("~/Views/Home/Index.cshtml", null);
            }

            return View("~/Views/Magazines/Index.cshtml", db.Magazines.ToList());
        }

        public ActionResult NewMagazine()
        {
            var clear = from n in db.Sellers
                        where n.SellerID == sellerID
                        select n;

            if (clear.First().clearance == 0)
            {
                return View("~/Views/Home/Index.cshtml", null);
            }

            return View("~/Views/Magazines/Create.cshtml", null);
        }

        public ActionResult addSeller()
        {
            String fname = Convert.ToString(Request["firstName"].ToString());
            String lname = Convert.ToString(Request["lastName"].ToString());
            String password = Convert.ToString(Request["password"].ToString());
            int clear = Convert.ToInt32(Request["clearance"].ToString());

            db.Sellers.Add(new Seller { FirstName = fname, LastName = lname, Password = password, clearance = clear });
            db.SaveChanges();

            return View("~/Views/Home/Index.cshtml", null);
        }

        public ActionResult SellerIndex()
        {
            var clear = from n in db.Sellers
                        where n.SellerID == sellerID
                        select n;

            if (clear.First().clearance == 0)
            {
                return View("~/Views/Home/Index.cshtml", null);
            }

            return View("~/Views/Sellers/Index.cshtml", db.Sellers.ToList());
        }

        public ActionResult NewEmployee()
        {
            var clear = from n in db.Sellers
                        where n.SellerID == sellerID
                        select n;

            if(clear.First().clearance==0)
            {
                return View("~/Views/Home/Index.cshtml", null);
            }

            return View("~/Views/Sellers/NewEmployee.cshtml", null);
        }

        public ActionResult Login()
        {
            int sid = Convert.ToInt32(Request["username"].ToString());
            string pw = Convert.ToString(Request["password"].ToString());
            LoginToken LT = new LoginToken();
            LT.password = pw;
            LT.sellerID = sid;

            if((LT.password==null)||(LT.password==null))
            {
                return View("~/Views/Home/Login.cshtml", null);
            }

            var sellerList = from n in db.Sellers
                             where n.SellerID == LT.sellerID
                             where n.Password == LT.password
                             select n;
            if(sellerList.ToList().Count == 0)
            {
                return View("~/Views/Home/Login.cshtml", null);
            }

            sellerID = LT.sellerID;
            return View("~/Views/Home/Index.cshtml", null);
            
        }

        // GET: Customers
        public ActionResult Index()
        {
            if(sellerID == -1)
            {
                return View("~/Views/Home/Login.cshtml", null);
            }
            return View(db.Customers.ToList());
        }

        public ActionResult CompleteCheckout()
        {
            string cardNum = Convert.ToString(Request["cardNum"].ToString());
            int expMonth = Convert.ToInt32(Request["cardExpMo"].ToString());
            int expYear = Convert.ToInt32(Request["cardExpYe"].ToString());

            var custInvoice = new Invoice {Total = ES.total, CardNumber = cardNum, ExpirationMonth = expMonth, ExpirationYear = expYear, SellerID=ES.SellerID, Email = ES.Email, OrderDate = DateTime.Now};
            Invoice newInvoice = db.Invoices.Add(custInvoice);
            db.SaveChanges();

            var customerList = from m in db.Customers
                           where m.Email == ES.Email
                           select m;
            

            foreach (var item in ES.magList)
            {
                db.InvoiceContains.Add(new InvoiceContain { InvoiceNumber = newInvoice.InvoiceNumber, MID =  item.MID});
                customerList.First().SubscriptionsBought = customerList.First().SubscriptionsBought + 1;
                db.SaveChanges();
            }

            CompleteInvoice CI = new CompleteInvoice {inv = newInvoice, magList = ES.magList };

            return View(CI);            
        }

        public ActionResult InvoiceDetails(Invoice inv)
        {
            CompleteInvoice cInv = new CompleteInvoice();

            cInv.inv = inv;

            var magazineList = from n in db.InvoiceContains
                               join m in db.Magazines on n.MID equals m.MID
                               where n.InvoiceNumber == inv.InvoiceNumber
                               select m;
            cInv.magList = magazineList.ToList();

            return View(cInv);
        }

        public ActionResult BillingInfo(EmailSeller ESeller)
        {
            return View();
        }

        public ActionResult CustomerInfo()
        {
            string email = Convert.ToString(Request["custEmail"].ToString());

            var custList = from n in db.Customers
                           where n.Email == email
                           select n;

            CustomerInvoice CI = new CustomerInvoice();
            CI.cust = custList.First();

            var invoiceList = from m in db.Invoices
                              where m.Email == email
                              select m;

            CI.invoices = invoiceList.ToList();

            return View(CI);
        }

        public ActionResult CustomerInfoString(string email)
        {
            var custList = from n in db.Customers
                           where n.Email == email
                           select n;

            CustomerInvoice CI = new CustomerInvoice();
            CI.cust = custList.First();

            var invoiceList = from m in db.Invoices
                              where m.Email == email
                              select m;

            CI.invoices = invoiceList.ToList();

            return View("CustomerInfo", CI);
        }

        public ActionResult Sell(EmailSeller token)
        {
            if (token.Email != null)
            {
                ES = token;
                ES.SellerID = sellerID;
            }
            else
            {
                var magName = Convert.ToString(Request["newMag"].ToString());

                var newMagazine = from a in db.Magazines
                                  where a.Name == magName
                                  select a;

                var newFirstMag = newMagazine.First();

                ES.magList.Add(newFirstMag);
                ES.total = ES.total + newFirstMag.Price;
                return View(ES);
            }
    

            return View(ES);
        }

        // GET: Customers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,FirstName,LastName,Address,City,SubscriptionsBought")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.SubscriptionsBought = 0;
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,FirstName,LastName,Address,City,SubscriptionsBought")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
