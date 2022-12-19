using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CountriesPopulation.Api.Dtos;
using CountriesPopulation.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountriesPopulation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
            private static readonly HttpClient client = new HttpClient();

            [HttpGet]
            public async Task<ActionResult<ApiResponse>> GetData()
            {
                try
                {
                    // Make an HTTP GET request to the API
                    var response = await client.GetAsync("https://countriesnow.space/api/v0.1/countries/population");

                    // Read the response content and deserialize it into an ApiResponse object
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                    // Return the data to the client
                    return Ok(apiResponse.Data);
                }
                catch (Exception ex)
                {
                    // Return an error response if there was a problem calling the API
                    return StatusCode(500, new { error = ex.Message });
                }
            }
        }
    
}

