using Library.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class GenreController : Controller
    {
        LibraryContext db = new LibraryContext();

        public ActionResult All()
        {
            var genres = db.Genres.OrderBy(n => n.Name);
            return View(genres.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            Genre genre = db.Genres.Find(id);
            return View(genre);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Genre genre)
        {
            db.Entry(genre).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("All");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Genre genre)
        {
            db.Genres.Add(genre);
            db.SaveChanges();
            return RedirectToAction("All");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            Genre genre = db.Genres.Find(id);
            if (genre != null)
            {
                db.Genres.Remove(genre);
                db.SaveChanges();
            }
            return RedirectToAction("All");
        }
    }
}