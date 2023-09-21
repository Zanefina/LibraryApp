using LibraryApp.Models;

namespace LibraryApp.Interfaces
{
    public interface IBookRepository
    {
        Task<ICollection<Book>>GetAllBooks();
        Task<Book>GetBookbyId(int bookId);
        Task<Book> CreateBook(Book toCreate);
        Task<Book> UpdateBook(Book book, int bookId);
        Task<Book>DeleteBook(int bookId);
    }
}
