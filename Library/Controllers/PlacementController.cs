using Library.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    [Authorize(Roles = "admin")]
    public class PlacementController : Controller
    {
        LibraryContext db = new LibraryContext();

        public ActionResult All()
        {
            var placements = db.Placements;
            return View(placements.ToList());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Placement placement = db.Placements.Find(id);
            return View(placement);
        }

        [HttpPost]
        public ActionResult Edit(Placement placement)
        {
            db.Entry(placement).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("All");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Placement placement)
        {
            db.Placements.Add(placement);
            db.SaveChanges();
            return RedirectToAction("All");
        }

        public ActionResult Delete(int id)
        {
            Placement placement = db.Placements.Find(id);
            if (placement != null)
            {
                db.Placements.Remove(placement);
                db.SaveChanges();
            }
            return RedirectToAction("All");
        }
    }
}