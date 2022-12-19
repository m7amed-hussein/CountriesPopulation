using System;
namespace CountriesPopulation.Api.Models
{
    public class Population : BaseEntity
    {
        public int Year { get; set; }
        public long Count { get; set; }
        public Country Country { get; set; }
        public Guid CountryId { get; set; }
    }
}

