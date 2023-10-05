using Microsoft.AspNetCore.Mvc;
using static Web_Library.StaticDetails;

namespace Web_Library.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; }

        public string Url { get; set; }
        public object Data { get; set; }

        public string AccessToken { get; set; }
    }
}
