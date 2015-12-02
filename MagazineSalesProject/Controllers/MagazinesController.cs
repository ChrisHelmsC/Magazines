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
    public class MagazinesController : Controller
    {
        private MagazineDataEntities db = new MagazineDataEntities();

        // GET: Magazines
        public ActionResult Index()
        {
            return View(db.Magazines.ToList());
        }

        // GET: Magazines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Magazine magazine = db.Magazines.Find(id);
            if (magazine == null)
            {
                return HttpNotFound();
            }
            return View(magazine);
        }

        // GET: Magazines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Magazines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,Genre,Price,Publisher")] Magazine magazine)
        {
            if (ModelState.IsValid)
            {
                Magazine mag = new Magazine
                {
                    Name = magazine.Name,
                    Description = magazine.Description,
                    Genre = magazine.Genre,
                    Price = magazine.Price,
                    Publisher = magazine.Publisher
                };

                var pubList = from n in db.Publishers
                              where n.Name == magazine.Publisher
                              select n;

                if (!pubList.Any())
                {
                    Publisher pub = new Publisher {Name = magazine.Name };
                    db.Publishers.Add(pub);
                    db.SaveChanges();
                }

                try
                {
                    db.Magazines.Add(mag);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception exception)
                {

                }
            }

            return View(magazine);
        }

        // GET: Magazines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Magazine magazine = db.Magazines.Find(id);
            if (magazine == null)
            {
                return HttpNotFound();
            }
            return View(magazine);
        }

        // POST: Magazines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MID,Name,Description,Genre,Price,Publisher")] Magazine magazine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(magazine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(magazine);
        }

        // GET: Magazines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Magazine magazine = db.Magazines.Find(id);
            if (magazine == null)
            {
                return HttpNotFound();
            }
            return View(magazine);
        }

        // POST: Magazines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Magazine magazine = db.Magazines.Find(id);
            db.Magazines.Remove(magazine);
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
