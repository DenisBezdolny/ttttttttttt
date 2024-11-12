using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Paymant_Module_NEOXONLINE.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        [HttpGet("GetInfo")]

        public async Task<IActionResult> Get()
        {
            return Ok("this is the paypal controller. here will be logic for interacting with the paypal api");
        }
    }
}
