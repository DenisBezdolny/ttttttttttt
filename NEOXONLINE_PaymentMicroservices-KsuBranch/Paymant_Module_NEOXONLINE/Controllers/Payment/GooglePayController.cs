using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Paymant_Module_NEOXONLINE.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class GooglePayController : ControllerBase
    {
        [HttpGet("GetInfo")]

        public async Task<IActionResult> Get()
        {
            return Ok("this is the google pay controller. here will be logic for interacting with the google api");
        }
    }
}
