using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Library.Controllers
{
    [Authorize(Roles = "admin")]
    public class SupplyController : Controller
    {
        LibraryContext db = new LibraryContext();

        public ActionResult All()
        {
            var supplies = db.Supplies.Include(r => r.Reader);              
            return View(supplies.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            var readers = db.Readers.OrderBy(l => l.Lastname).AsEnumerable().Select(s => new { Id = s.ReaderId, FIO = string.Format("{0} {1} {2}", s.Lastname, s.Name, s.Patronymic) }).ToList();
            var books = db.Books.Where(a => a.Amount > 0).OrderBy(t => t.Title).AsEnumerable().Select(s => new { Id = s.ISBN, BookInfo = $"{s.ISBN} {s.Title}" }).ToList();

            ViewBag.Readers = new SelectList(readers, "Id", "FIO");
            ViewBag.Books = new MultiSelectList(books, "Id", "BookInfo");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Supply supply, IEnumerable<string> books)
        {
            Supply result = new Supply();

            result.Date = supply.Date;
            result.ReaderId = supply.ReaderId;
            foreach (var sel_book in books)
            {
                Book book = db.Books.Find(sel_book);
                book.Amount -= 1;
                db.Entry(book).State = EntityState.Modified;
                result.Books.Add(book);
            }

            db.Supplies.Add(result);
            db.SaveChanges();
            return RedirectToAction("All");
        }

        public ActionResult Delete(int id)
        {
            Supply supply = db.Supplies.Find(id);
            if (supply != null)
            {
                foreach (var spl_book in supply.Books)
                {
                    Book book = db.Books.Find(spl_book.ISBN);
                    book.Amount += 1;
                    db.Entry(book).State = EntityState.Modified;
                }
                db.Supplies.Remove(supply);
                db.SaveChanges();
            }
            return RedirectToAction("All");
        }
    }
}