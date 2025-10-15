using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EksiSozluk.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        //public Guid? UserId => new(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value); // JWT Token ile alıyoruz bunu
        public Guid UserId => Guid.NewGuid(); // JWT Token ile alıyoruz bunu
    }
}
