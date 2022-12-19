using System;
namespace CountriesPopulation.Api.Dtos
{
    public class CountryDto
    {
        public string Country { get; set; }
        public string Code { get; set; }
        public string Iso3 { get; set; }
        public List<PopulationDto> populationCounts { get; set; }
    }
}

