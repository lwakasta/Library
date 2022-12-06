using Library.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class AuthorController : Controller
    {
        LibraryContext db = new LibraryContext();

        public ActionResult All()
        {
            var authors = db.Authors.OrderBy(l => l.Lastname);
            return View(authors.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            Author author = db.Authors.Find(id);
            return View(author);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Author author)
        {
            db.Entry(author).State = EntityState.Modified;
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
        public ActionResult Create(Author author)
        {
            db.Authors.Add(author);
            db.SaveChanges();
            return RedirectToAction("All");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            Author author = db.Authors.Find(id);
            if (author != null)
            {
                db.Authors.Remove(author);
                db.SaveChanges();
            }
            return RedirectToAction("All");
        }
    }
}