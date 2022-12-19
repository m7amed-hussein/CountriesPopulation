using System;
using CountriesPopulation.Api.Dtos;
using CountriesPopulation.Api.EfCore;
using CountriesPopulation.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CountriesPopulation.Api.Repositories
{
    public class CountryRepository : ICountryRepository<Country>
    {
        private readonly CountriesPopulationDbContext _dbContext;
        public CountryRepository(CountriesPopulationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task  AddManyAsync(List<Country> countries)
        {
            await _dbContext.Set<Country>().AddRangeAsync(countries);
            _dbContext.SaveChanges();
        }
        public async Task AddAsync(Country country)
        {
            await _dbContext.Set<Country>().AddAsync(country);
        }
        public async Task SaveChangesAsync(){
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Country country)
        {
            var countryEntity = await _dbContext.Countries.Include(c=>c.PopulationCount).FirstOrDefaultAsync(c => c.Id == country.Id);
            if(countryEntity == null)
                throw new EntityNotFoundException();
            countryEntity.Name = country.Name;
            countryEntity.Code = country.Code;
            countryEntity.Iso3 = country.Iso3;
            UpdatePopulation(countryEntity, country.PopulationCount);
        }
        private void UpdatePopulation(Country country, List<Population> population)
        {
            var countryPopulation = country.PopulationCount;
            if(population == null)
                countryPopulation = new List<Population>();
            if(countryPopulation != null){
                foreach(var pop in countryPopulation)
            {
                _dbContext.Populations.Remove(pop);
            }
            }
            foreach(var pop in population)
            {
                pop.CountryId = country.Id;
                _dbContext.Populations.Add(pop);
            }

        }
        public async Task<List<Country>> GetPagedListAsync(int page)
        {
            return  await ( _dbContext.Countries.Include(c =>c.PopulationCount).AsNoTracking().Skip((page - 1) * 50).Take(50).ToListAsync());
        }
        public async Task<List<Country>> GetListAsync()
        {
            return  await ( _dbContext.Countries.Include(c =>c.PopulationCount).AsNoTracking().ToListAsync());
        }
        public async Task<Country> GetAsync(Guid id)
        {
            return await _dbContext.Countries.Include(c => c.PopulationCount).FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Country> GetAsync(string search)
        {
            return await _dbContext.Countries.Include(c => c.PopulationCount).FirstOrDefaultAsync(c => c.Name == search || c.Code == search);
        }
    }
}

