using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class Placement
    {
        [Key]
        public int PlacementId { get; set; }
        public string Hall { get; set; }
        public string Section { get; set; }
        public string Rack { get; set; }
        public string Shelf { get; set; }
    }
}