using LibraryApp.Models;
using LibraryApp.Models.DTO;

namespace Web_Library.Services
{
    public interface IBookService
    {
        Task<T> GetAllBooks<T>();

        Task<T> GetBookById<T>(int bookId);

        Task<T> CreateBookAsync<T>(BookDTO bookDTO);
        Task<T> UpdateBookAsync<T>(BookDTO bookDTO);

        Task<T> DeleteBookAsync<T>(int bookId);
    }
}
