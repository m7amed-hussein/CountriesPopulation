using System;
namespace CountriesPopulation.Api.Models
{
    public class Population
    {
        public Guid Id { get; set; }
        public int Year { get; set; }
        public int Count { get; set; }
        public Country Country { get; set; }
        public Guid CountryId { get; set; }
    }
}

