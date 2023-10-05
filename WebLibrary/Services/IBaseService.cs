using WebLibrary.Models;

namespace WebLibrary.Services
{
    public interface IBaseService : IDisposable
    {
        ResponsDto responsModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
