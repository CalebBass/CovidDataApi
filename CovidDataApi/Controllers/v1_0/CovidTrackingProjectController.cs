using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CovidData.Api.Config;
using CovidData.Api.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CovidData.Api.Controllers.v1_0
{
    [Authorize]
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

        /// <summary>
        /// Gets the United States' current Covid data
        /// </summary>
        /// <returns></returns>
        [HttpGet("usa/current"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(List<CovidTrackingUsaCurrentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CovidTrackingUsaCurrentResponse>>> GetAllUsaCurrentData()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_covidTrackingConfig.UsaBaseUrl}");

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode);
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

        /// <summary>
        /// Gets the specified state's (by 2 letter abbreviation) current Covid data
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
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
                    return StatusCode((int)response.StatusCode);
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
        /// <summary>
        /// Gets a specified state's historical Covid data
        /// </summary>
        /// <param name="state"></param>
        /// <returns>List of stats by day since record=keeping began</returns>
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
                    return StatusCode((int)response.StatusCode);
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

        /// <summary>
        /// Gets a state's Covid data for a specific date
        /// </summary>
        /// <param name="state">Two-letter state abbreviation</param>
        /// <param name="date">Date format (yyyymmddd): 20201127</param>
        /// <returns></returns>
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
                    return StatusCode((int)response.StatusCode);
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
