using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasData(
                    new Book { BookId = 1, Title = "The Secret Garden", Author = "Frances Hodgson Burnett", Genre = "Children's Fiction", PublishedOn = new DateTime(1911, 10, 1), Description = "A timeless classic about the power of nature and friendship.", Availability = true },
                    new Book { BookId = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", Genre = "Fiction", PublishedOn = new DateTime(1960, 7, 11), Description = "A compelling story of racial injustice and moral growth in the American South.", Availability = false },
                    new Book { BookId = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Genre = "Classic Literature", PublishedOn = new DateTime(1925, 4, 10), Description = "A tale of decadence, obsession, and the American Dream during the Roaring Twenties.", Availability = true },
                    new Book { BookId = 4, Title = "The Hobbit", Author = "J.R.R. Tolkien", Genre = "Fantasy", PublishedOn = new DateTime(1937, 9, 21), Description = "The adventurous journey of Bilbo Baggins, a hobbit, to reclaim treasure guarded by a dragon.", Availability = false },
                    new Book { BookId = 5, Title = "The Catcher in the Rye", Author = "J.D. Salinger", Genre = "Coming-of-Age", PublishedOn = new DateTime(1951, 7, 16), Description = "Holden Caulfield's existential journey through the streets of New York City.", Availability = true }

            );
        }
    }
}
