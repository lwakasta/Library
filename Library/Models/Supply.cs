using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class Supply
    {
        [Key]
        public int SupplyId { get; set; }
        public string Date { get; set; }
        public int ReaderId { get; set; }
        public Reader Reader { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public Supply()
        {
            Books = new List<Book>();
        }
    }
}