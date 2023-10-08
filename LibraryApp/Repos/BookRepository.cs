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
            var result = await _context.Books.FirstOrDefaultAsync(x => x.BookId == bookId);
            if (result != null)
            {
                _context.Books.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }


        public async Task<Book> UpdateBook(Book book)
        {
            var result = await _context.Books.FirstOrDefaultAsync(b => b.BookId == book.BookId);
            if (result != null)
            {
                result.Title = book.Title;
                result.Author = book.Author;
                result.Genre = book.Genre;
                result.PublishedOn = book.PublishedOn;
                result.Description = book.Description;
                result.Availability = book.Availability;

                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
