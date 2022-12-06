using Library.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Library.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Reader reader = null;
                Librarian librarian = null;
                using (LibraryContext db = new LibraryContext())
                {
                    reader = db.Readers.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                    librarian = db.Librarians.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                }
                if (reader != null || librarian != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("All", "Book");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет!");
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Reader reader = null;
                using (LibraryContext db = new LibraryContext())
                {
                    reader = db.Readers.FirstOrDefault(u => u.Login == model.Login);
                }
                if (reader == null)
                {
                    using (LibraryContext db = new LibraryContext())
                    {
                        db.Readers.Add(new Reader { Login = model.Login, Password = model.Password, Name = model.Name, Lastname = model.Lastname, Patronymic = model.Patronymic, Birth = model.Birth, RoleId = 1 });
                        db.SaveChanges();
                        reader = db.Readers.Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();
                    }

                    // если пользователь удачно добавлен в бд
                    if (reader != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true);
                        return RedirectToAction("All", "Book");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }
            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("All", "Book");
        }

        public ActionResult Personal()
        {
            Reader reader = null;
            using (LibraryContext db = new LibraryContext())
            {
                reader = db.Readers.FirstOrDefault(u => u.Login == User.Identity.Name);
                var supplies = db.Supplies.Where(r => r.ReaderId == reader.ReaderId).ToList();

                List<ICollection<Book>> books = new List<ICollection<Book>>();
                foreach (var s in supplies)
                {
                    books.Add(s.Books);
                }
                ICollection<Book> sup_books = new List<Book>();
                for (int i = 0; i < books.Count; i++)
                {
                    for (int j = 0; j < books[i].Count; j++)
                    {
                        sup_books.Add(books[i].ElementAt(j));
                    }
                }

                ViewBag.Books = sup_books;
                return View(reader);
            }
        }

        [HttpGet]
        public ActionResult ChangeInfo()
        {
            using (LibraryContext db = new LibraryContext())
            {
                Reader reader = db.Readers.FirstOrDefault(u => u.Login == User.Identity.Name);
                return View(reader);
            }
        }

        [HttpPost]
        public ActionResult ChangeInfo(Reader reader)
        {
            using (LibraryContext db = new LibraryContext())
            {
                db.Entry(reader).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Personal");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(RegisterModel model)
        {
            if (model.Password == model.ConfirmPassword)
            {
                using (LibraryContext db = new LibraryContext())
                {
                    var reader = db.Readers.FirstOrDefault(u => u.Login == User.Identity.Name);
                    reader.Password = model.Password;
                    db.Entry(reader).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Personal");
            }            
            return View(model);
        }
    }
}