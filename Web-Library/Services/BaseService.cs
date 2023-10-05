using Newtonsoft.Json;
using System.Text;
using Web_Library.Models;

namespace Web_Library.Services
{
    public class BaseService : IBaseService
    {
        public ResponsDto responsModel { get; set; }
        public IHttpClientFactory _httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this._httpClient = httpClient;
            this.responsModel = new ResponsDto();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {

                var client = _httpClient.CreateClient("BookAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();

                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }

                HttpResponseMessage apiResp = null;
                switch (apiRequest.ApiType)
                {
                    case StaticDetails.ApiType.GET:
                        message.Method = HttpMethod.Get;
                        break;
                    case StaticDetails.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case StaticDetails.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case StaticDetails.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                }
                apiResp = await client.SendAsync(message);

                var apiContent = await apiResp.Content.ReadAsStringAsync();
                var apiResponsDto = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponsDto;
            }

            catch (Exception e)
            {

                var dto = new ResponsDto
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false

                };

                var res = JsonConvert.SerializeObject(dto);
                var apiResponsDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponsDto;
            }

        }
    }
}
