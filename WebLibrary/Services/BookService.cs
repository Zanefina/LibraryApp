using LibraryApp.Models.DTO;

namespace WebLibrary.Services
{
    public class BookService : BaseService, IBookService
    {
        private readonly IHttpClientFactory _clientFactory;
        public BookService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> CreateBookAsync<T>(BookDTO bookDTO)
        {
            return await this.SendAsync<T>(new Models.ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = bookDTO,
                Url = StaticDetails.BookApiBase + "/api/books/",
                AccessToken = ""
            });
        }

        public async Task<T> DeleteBookAsync<T>(int bookId)
        {
            return await this.SendAsync<T>(new Models.ApiRequest
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.BookApiBase + "/api/books/" + bookId,
                AccessToken = ""
            });
        }

        public Task<T> GetAllBooks<T>()
        {
            return this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.BookApiBase + "/api/books/",
                AccessToken = ""
            });
        }

        public async Task<T> GetBookById<T>(int bookId)
        {
            return await this.SendAsync<T>(new Models.ApiRequest
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.BookApiBase + "/api/books/" + bookId,
                AccessToken = ""
            });
        }

        public async Task<T> UpdateBookAsync<T>(BookDTO bookDTO)
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = bookDTO,
                Url = StaticDetails.BookApiBase + "/api/books/",
                AccessToken = ""
            });
        }
    }
}
