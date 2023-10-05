using System.Numerics;
using static WebLibrary.StaticDetails;

namespace WebLibrary.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; }

        public string Url { get; set; }
        public object Data { get; set; }

        public string AccessToken { get; set; }
    }
}
