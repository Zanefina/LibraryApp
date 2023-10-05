using Web_Library.Models;

namespace Web_Library.Services
{
    public interface IBaseService : IDisposable
    {
        ResponsDto responsModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
