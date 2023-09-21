using LibraryApp.Data;
using LibraryApp.Interfaces;
using LibraryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Repos
{
    public class BookRepository : IBookRepository
    {
        private readonly Context _context;
        public BookRepository(Context context)
        {
            _context = context;
        }

        public async Task<ICollection<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }


        public async Task<Book> GetBookbyId(int bookId)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
        }

        public async Task<Book> CreateBook(Book toCreate)
        {
            toCreate.Title = toCreate.Title;
            toCreate.Author = toCreate.Author;
            toCreate.PublishedOn = DateTime.Now;
            toCreate.Genre = toCreate.Genre;
            toCreate.Description = toCreate.Description;
            toCreate.Availability = true;
            _context.Books.Add(toCreate);
            await _context.SaveChangesAsync();
            return toCreate;
        }

        public async Task <Book> DeleteBook(int bookId)
        {
            var book = await _context.Books
            .FirstOrDefaultAsync(b => b.BookId == bookId);

            if (book == null)
            {
                return null; // Return null when the book is not found.
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return book; // Return the deleted book.
        }


        public async Task<Book> UpdateBook(Book book, int bookId)
        {
            var updatedBook = await _context.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
            if (updatedBook != null)
            {
                updatedBook.BookId = bookId;
                updatedBook.Title = book.Title;
                updatedBook.Author = book.Author;
                updatedBook.Genre = book.Genre;
                updatedBook.PublishedOn = book.PublishedOn;
                updatedBook.Availability = book.Availability;

                await _context.SaveChangesAsync();
                return updatedBook;
            }
            return null;
        }
    }
}
