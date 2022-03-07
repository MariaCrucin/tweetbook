using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TweetBook.Filters;

namespace TweetBook.Controllers
{
    [ApiKeyAuth]
    [ApiController]
    public class SecretController : ControllerBase
    {
        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            return Ok("I have no secrets");
        }
    }
}
