using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class BookCart
    {
        public List<Book> Books = new List<Book>();

        public void AddItem(Book book)
        {
            Book line = Books
                .Where(b => b.ISBN == book.ISBN)
                .FirstOrDefault();

            if (line == null)
            {
                Books.Add(book);
            }
        }

        public void RemoveItem(Book book)
        {
            Books.RemoveAll(b => b.ISBN == book.ISBN);
        }
    }
}