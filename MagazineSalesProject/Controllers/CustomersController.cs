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

        // GET: Customers
        public ActionResult Index()
        {
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

        public ActionResult Sell(EmailSeller token)
        {
            if (token.Email != null)
            {
                ES = token;
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
