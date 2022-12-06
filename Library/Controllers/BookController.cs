using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Library.Models;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        LibraryContext db = new LibraryContext();
        
        public ActionResult All()
        {
            var books = db.Books.Include(a => a.Author)
                                .Include(g => g.Genre)
                                .Include(p => p.Placement)
                                .OrderBy(t => t.Title);
            return View(books.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(string id)
        {
            Book book = db.Books.Include(a => a.Author)
                                .Include(g => g.Genre)
                                .Include(p => p.Placement)
                                .FirstOrDefault(i => i.ISBN == id);

            var authors = db.Authors.OrderBy(l => l.Lastname).AsEnumerable().Select(s => new { Id = s.AuthorId, FIO = string.Format("{0} {1} {2}", s.Lastname, s.Name, s.Patronymic) }).ToList();
            var placements = db.Placements.AsEnumerable().Select(s => new { Id = s.PlacementId, Place = $"{s.Hall}, {s.Section}, стеллаж: {s.Rack}, полка: {s.Shelf}" }).ToList();

            ViewBag.Authors = new SelectList(authors, "Id", "FIO");
            ViewBag.Genres = new SelectList(db.Genres.OrderBy(n => n.Name), "GenreId", "Name");
            ViewBag.Placements = new SelectList(placements, "Id", "Place");

            return View(book);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Book book)
        {
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("All");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            var authors = db.Authors.OrderBy(l => l.Lastname).AsEnumerable().Select(s => new { Id = s.AuthorId, FIO = string.Format("{0} {1} {2}", s.Lastname, s.Name, s.Patronymic) }).ToList();
            var placements = db.Placements.AsEnumerable().Select(s => new { Id = s.PlacementId, Place = $"{s.Hall}, {s.Section}, стеллаж: {s.Rack}, полка: {s.Shelf}" }).ToList();

            ViewBag.Authors = new SelectList(authors, "Id", "FIO");
            ViewBag.Genres = new SelectList(db.Genres.OrderBy(n => n.Name), "GenreId", "Name");
            ViewBag.Placements = new SelectList(placements, "Id", "Place");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(Book book)
        {
            db.Books.Add(book);
            db.SaveChanges();
            return RedirectToAction("All");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(string id)
        {
            Book book = db.Books.Find(id);
            if (book != null)
            {
                db.Books.Remove(book);
                db.SaveChanges();
            }
            return RedirectToAction("All");
        }
    }
}