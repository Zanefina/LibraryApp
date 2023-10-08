using System.Net;

namespace LibraryApp.Models
{
    public class ApiResponse
    {
        
        public ApiResponse()
        {
            ErrorMessages = new List<string>();
        }
        public List<string> ErrorMessages { get; set; }
        public bool IsSuccess { get; set; }
        public Object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }

    }
}

