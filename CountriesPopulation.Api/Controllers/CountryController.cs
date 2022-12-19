using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CountriesPopulation.Api.Dtos;
using CountriesPopulation.Api.Models;
using CountriesPopulation.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountriesPopulation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
            private readonly IHttpClientFactory _clientFactory;
            private readonly ICountryService _service;

            public CountryController(ICountryService service , IHttpClientFactory clientFactory)
            {
                _service = service;
                _clientFactory = clientFactory;
            }
            //Get country
            [HttpGet]
            [Route("Get")]
            public async Task<ActionResult<Country>> GetCountry(Guid id)
            {
                try
                {
                    var country = await _service.Get(id);
                    if(country == null)
                        return NotFound(new { error = "Country not found" });
                    return Ok(country);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
            [HttpGet]
            [Route("GetPaged")]
            public async Task<ActionResult<List<Country>>> GetCountries(int page)
            {
                try
                {
                    var countries = await _service.GetList(page);
                    return Ok(countries);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
            //Get country by name or code
            [HttpGet]
            [Route("GetByNameOrCode")]
            public async Task<ActionResult<Country>> GetCountry(string search)
            {
                try
                {
                    var country = await _service.Get(search);
                    if(country == null)
                        return NotFound(new { error = "Country not found" });
                    return Ok(country);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
            [HttpPost]
            [Route("Sync")]
            public async Task<ActionResult> SyncCountries()
            {
                var client = _clientFactory.CreateClient();
                try
                {
                    // Make an HTTP GET request to the API
                    var response = await client.GetAsync("https://countriesnow.space/api/v0.1/countries/population");

                    // Read the response content and deserialize it into an ApiResponse object
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                    await _service.SyncFromApi(apiResponse.Data);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
    
}

