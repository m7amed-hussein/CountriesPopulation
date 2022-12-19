using System;
namespace CountriesPopulation.Api.Models
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Iso3 { get; set; }

        public List<Population> PopulationCount { get; set; }
    }
}

