using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    public class HandshakeController : ControllerBase
    {
        [Route("api/Handshake/hi")]
        [HttpGet]
        public string hello()
        {
            return "salam";
        }

        [Route("api/Handshake/bye")]
        [HttpGet]
        public string goodby()
        {
            return "khodafez";
        }
    }
}
