using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CovidData.Api.Config;
using CovidData.Api.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CovidData.Api.Controllers.v1_0
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CdcCovidController : ControllerBase
    {
        private readonly IOptionsSnapshot<CdcApiConfig> _cdcApiConfig;
        private readonly IHttpClientFactory _httpClientFactory;

        public CdcCovidController(IOptionsSnapshot<CdcApiConfig> cdcApiConfig, IHttpClientFactory httpClientFactory)
        {
            _cdcApiConfig = cdcApiConfig;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("allusa"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(CdcDataResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CdcDataResponse>> GetAllUsaCdcData()
        {

            var client = _httpClientFactory.CreateClient("CdcApi");
            var request = new HttpRequestMessage(HttpMethod.Get, string.Empty);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStreamAsync();

                var deserializedCdcResponse = await JsonSerializer.DeserializeAsync<CdcDataResponse>(data);
                return Ok(data);
            }

            return StatusCode(500);

        }

//
//        [HttpGet("allusa"), MapToApiVersion("1.0")]
//        [ProducesResponseType(typeof(CdcDataResponse), StatusCodes.Status200OK)]
//        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//        public async Task<ActionResult<CdcDataResponse>> GetAllUsaCdcData()
//        {
//
//            var client = _httpClientFactory.CreateClient("CdcApi");
//            var request = new HttpRequestMessage(HttpMethod.Get, string.Empty);
//
//            var response = await client.SendAsync(request);
//
//            if (response.IsSuccessStatusCode)
//            {
//                var data = await response.Content.ReadAsStreamAsync();
//
//                var deserializedCdcResponse = await JsonSerializer.DeserializeAsync<CdcDataResponse>(data);
//                return Ok(data);
//            }
//
//            return StatusCode(500);
//
//        }



    }


}
