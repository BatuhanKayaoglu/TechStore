using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace EksiSozluk.Api.WebApi.Middlewares.Filter.GlobalExceptionHandler
{
    public class ExceptionModel : ErrorStatusCode
    {
        public IEnumerable<string> Errors { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class ErrorStatusCode
    {
        public int StatusCode { get; set; }
    }
}
