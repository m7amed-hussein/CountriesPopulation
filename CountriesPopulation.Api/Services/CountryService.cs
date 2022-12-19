using System;
using CountriesPopulation.Api.Dtos;
using CountriesPopulation.Api.Models;
using CountriesPopulation.Api.Repositories;

namespace CountriesPopulation.Api.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository<Country> _repository;
        public CountryService(ICountryRepository<Country> repository)
        {
            _repository = repository;
        }
        public void AddMany(List<CountryDto> countries)
        {
            var Countries = new List<Country>();
            foreach(var country in countries)
            {
                Countries.Add(new Country()
                {
                    Name = country.Country,
                    Code = country.Code,
                    Iso3 = country.Iso3,
                    PopulationCount = country.populationCounts.Select(x => new Population()
                    {
                        Year = x.Year,
                        Count = x.Value
                    }).ToList()     
                });
            }
            _repository.AddManyAsync(Countries);
        }
        public async Task<List<CountryDto>> GetList(int page)
        {
            var countries =  await _repository.GetPagedListAsync(page);
            var countriesDto = countries.Select(x => new CountryDto()
            {
                Country = x.Name,
                Code = x.Code,
                Iso3 = x.Iso3,
                populationCounts = x.PopulationCount.Select(y => new PopulationDto()
                {
                    Year = y.Year,
                    Value = y.Count
                }).ToList()
            }).ToList();
            return countriesDto;
        }
        public async Task<CountryDto> Get(Guid id)
        {
            var country =  await _repository.GetAsync(id);
            if(country == null)
                return null;
            return new CountryDto()
            {
                Country = country.Name,
                Code = country.Code,
                Iso3 = country.Iso3,
                populationCounts = country.PopulationCount.Select(y => new PopulationDto()
                {
                    Year = y.Year,
                    Value = y.Count
                }).ToList()
            };
        }
        public async Task<CountryDto> Get(string search)
        {
            var country =  await _repository.GetAsync(search);
            if(country == null)
                return null;
            return new CountryDto()
            {
                Country = country.Name,
                Code = country.Code,
                Iso3 = country.Iso3,
                populationCounts = country.PopulationCount.Select(y => new PopulationDto()
                {
                    Year = y.Year,
                    Value = y.Count
                }).ToList()
            };
        }
        public async Task SyncFromApi(List<CountryDto> countriesDto)
        {
            //All countries in database
            var countriesDictionary = (await _repository.GetListAsync()).Select(x => new {x.Name, x.Id}).ToDictionary(x => x.Name, x => x.Id);
            foreach( var countryDto in countriesDto){
                var country = new Country()
                    {
                        Name = countryDto.Country,
                        Code = countryDto.Code,
                        Iso3 = countryDto.Iso3,
                        PopulationCount = countryDto.populationCounts.Select(x => new Population()
                        {
                            Year = x.Year,
                            Count = x.Value
                        }).ToList()     
                    };
                if(!countriesDictionary.ContainsKey(countryDto.Country))
                {
                    await _repository.AddAsync(country);
                }else{
                    country.Id = countriesDictionary[country.Name];
                    await _repository.UpdateAsync(country);
                }
            }

            await _repository.SaveChangesAsync();
        }
    }
}

