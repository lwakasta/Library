using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Placement> Placements { get; set; } 
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supply>().HasMany(s => s.Books)
                .WithMany(r => r.Supplies)
                .Map(t => t.MapLeftKey("SupplyId")
                .MapRightKey("ISBN")
                .ToTable("SupplyBooks"));

            modelBuilder.Entity<Reservation>().HasMany(s => s.Books)
                .WithMany(r => r.Reservations)
                .Map(t => t.MapLeftKey("ReservationId")
                .MapRightKey("ISBN")
                .ToTable("ReservationBooks"));

            base.OnModelCreating(modelBuilder);
        }
    }
}