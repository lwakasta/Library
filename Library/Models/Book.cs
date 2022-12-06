using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class Book
    {
        [Key]
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public int Amount { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int PlacementId { get; set; }
        public Placement Placement { get; set; }
        public virtual ICollection<Supply> Supplies { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public Book()
        {
            Supplies = new List<Supply>();
            Reservations = new List<Reservation>();
        }
    }
}