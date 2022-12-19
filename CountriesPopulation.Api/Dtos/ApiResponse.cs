using System;
namespace CountriesPopulation.Api.Dtos
{
    public class ApiResponse
    {
        public bool Error { get; set; }
        public string Msg { get; set; }
        public List<CountryDto> Data { get; set; }
    }
}

