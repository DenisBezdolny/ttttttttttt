using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ILogger = Serilog.ILogger;

namespace Paymant_Module_NEOXONLINE.Controllers.CurrancyLayer
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyLayerController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        public CurrencyLayerController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public class Currency
        {
            public string Source { get; set; }
            public Dictionary<string, decimal> Quotes { get; set; }
        }


        [HttpGet("rates")]
        public async Task<IActionResult> GetRates()
        {
            string apiKey = "1762131c1ae91be7213a0889f595b57a";
            string currencies = "USD,BYN,RUB";
            string currencyBase = "EUR";
            string format = "1";
            string url = $"http://api.currencylayer.com/live?access_key={apiKey}&source={currencyBase}&currencies={currencies}&format={format}";

            HttpResponseMessage responseMessage = await _httpClient.GetAsync(url);

            if (!responseMessage.IsSuccessStatusCode)
            {
                return StatusCode((int)responseMessage.StatusCode, "Error when receiving data from CurrencyLayer");
            }

            string resposeBoby = await responseMessage.Content.ReadAsStringAsync();
            
            JObject json = JObject.Parse(resposeBoby);
            string log = json.ToString();


            var currency = new Currency
            {
                Source = json["source"].ToString(),
                Quotes = json["quotes"].ToObject<Dictionary<string, decimal>>()
            };




            return Ok(currency);
        }
    }
}
