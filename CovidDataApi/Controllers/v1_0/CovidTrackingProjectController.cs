using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CovidData.Api.Config;
using CovidData.Api.Response;
using Microsoft.Extensions.Options;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CovidData.Api.Controllers.v1_0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CovidTrackingProjectController : ControllerBase
    {
        private readonly CovidTrackingProjectApiConfig _covidTrackingConfig;
        private readonly IHttpClientFactory _httpClientFactory;

        public CovidTrackingProjectController(IOptionsSnapshot<CovidTrackingProjectApiConfig> covidTrackingConfig, IHttpClientFactory httpClientFactory)
        {
            _covidTrackingConfig = covidTrackingConfig.Value;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("usa/current"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(List<CovidTrackingUsaCurrentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CovidTrackingUsaCurrentResponse>>> GetAllUsaCdcData()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_covidTrackingConfig.UsaBaseUrl}");

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(500);
                }

                var data = await response.Content.ReadAsStreamAsync();

                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                var deserializedCdcResponse = await JsonSerializer.DeserializeAsync<List<CovidTrackingUsaCurrentResponse>>(data, options);
                return Ok(deserializedCdcResponse);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get data", e.InnerException);
            }
        }

        [HttpGet("states/{state}/current"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(CovidTrackingStateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CovidTrackingStateResponse>> GetCurrentStateData(string state)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CdcApi");
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_covidTrackingConfig.UsaStatesBaseUrl}/{state}/current.json");

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(500);
                }

                var data = await response.Content.ReadAsStreamAsync();

                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                var deserializedCdcResponse = await JsonSerializer.DeserializeAsync<CovidTrackingStateResponse>(data, options);
                return Ok(deserializedCdcResponse);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get data", e.InnerException);
            }
        }

        // accept a date or range and filter on the backend
        [HttpGet("states/{state}/historical"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(List<CovidTrackingStateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CovidTrackingStateResponse>>> GetHistoricalStateData(string state)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_covidTrackingConfig.UsaStatesBaseUrl}/{state}/daily.json");

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(500);
                }

                var data = await response.Content.ReadAsStreamAsync();

                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                var deserializedCdcResponse = await JsonSerializer.DeserializeAsync<List<CovidTrackingStateResponse>>(data, options);
                return Ok(deserializedCdcResponse);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get data", e.InnerException);
            }
        }

        [HttpGet("states/{state}/historical/{date}"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(CovidTrackingStateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CovidTrackingStateResponse>> GetHistoricalStateData(string state, string date)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_covidTrackingConfig.UsaStatesBaseUrl}/{state}/{date}.json");

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(500);
                }

                var data = await response.Content.ReadAsStreamAsync();

                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                var deserializedCdcResponse = await JsonSerializer.DeserializeAsync<CovidTrackingStateResponse>(data, options);
                return Ok(deserializedCdcResponse);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get data", e.InnerException);
            }
        }

    }
}
