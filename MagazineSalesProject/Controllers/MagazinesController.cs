using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagazineSalesProject.Models;

namespace MagazineSalesProject.Controllers
{
    public class MagazinesController : Controller
    {
        MagazineDataEntities db = new MagazineDataEntities(); //Get a hold on DB

        public ActionResult Index()
        {
            var magazines = from m in db.Magazines
                            where m.MID > 2
                            select m;

            return View(magazines.ToList());
        }
    }
}