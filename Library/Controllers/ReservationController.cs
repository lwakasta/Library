using Library.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class ReservationController : Controller
    {
        LibraryContext db = new LibraryContext();

        [Authorize(Roles = "user")]
        public ActionResult MyReservation()
        {
            Reader reader = db.Readers.FirstOrDefault(l => l.Login == User.Identity.Name);
            Reservation reservation = db.Reservations.FirstOrDefault(i => i.ReaderId == reader.ReaderId);
            if (reservation == null)
            {
                ViewBag.Books = null;
            } else
            {
                ViewBag.Books = reservation.Books.ToList();
            }
            
            return View();
        }

        [Authorize(Roles = "user")]
        public ActionResult Reserve(string id)
        {
            Reader reader = db.Readers.FirstOrDefault(l => l.Login == User.Identity.Name);
            Book book = db.Books.FirstOrDefault(i => i.ISBN == id);

            Reservation reservation = db.Reservations.FirstOrDefault(r => r.ReaderId == reader.ReaderId);
            if (reservation == null)
            {
                reservation = new Reservation { ReaderId = reader.ReaderId, Date = DateTime.Now.ToString().Substring(0, 10) };
                db.Reservations.Add(reservation);
            }

            if (!reservation.Books.Contains(book))
            {
                reservation.Books.Add(book);
                book.Amount -= 1;
                db.Entry(book).State = EntityState.Modified;
            }            
            db.SaveChanges();

            return RedirectToAction("All", "Book");
        }

        [Authorize(Roles = "user")]
        public ActionResult Cancel(string id)
        {
            Reader reader = db.Readers.FirstOrDefault(l => l.Login == User.Identity.Name);
            Book book = db.Books.FirstOrDefault(i => i.ISBN == id);

            Reservation reservation = db.Reservations.First(r => r.ReaderId == reader.ReaderId);
            reservation.Books.Remove(book);
            book.Amount += 1;
            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("MyReservation");
        }

        [Authorize(Roles = "admin")]
        public ActionResult AllReservations()
        {
            var reservations = db.Reservations.Include(b => b.Books)
                                              .Include(r => r.Reader);

            return View(reservations.ToList());
        }

        [Authorize(Roles = "admin")]
        public ActionResult MoveToSupplies(int id)
        {
            Reservation reservation = db.Reservations.FirstOrDefault(i => i.ReservationId == id);
            Supply supply = new Supply { ReaderId = reservation.ReaderId, Date = DateTime.Now.ToString().Substring(0, 10) };
            foreach (var book in reservation.Books)
            {
                supply.Books.Add(book);
            }
            db.Supplies.Add(supply);
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("AllReservations");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            Reservation reservation = db.Reservations.FirstOrDefault(i => i.ReservationId == id);
            foreach (var book in reservation.Books)
            {
                Book bk = db.Books.FirstOrDefault(i => i.ISBN == book.ISBN);
                bk.Amount += 1;
                db.Entry(bk).State = EntityState.Modified;
            }
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("AllReservations");
        }
    }
}