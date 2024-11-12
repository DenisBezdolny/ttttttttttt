using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Paymant_Module_NEOXONLINE.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class SepaController : ControllerBase
    {
        [HttpGet("GetInfo")]

        public async Task<IActionResult> Get()
        {
            return Ok("this is the sepa controller. here will be the logic for processing sepa payments");
        }
    }
}
