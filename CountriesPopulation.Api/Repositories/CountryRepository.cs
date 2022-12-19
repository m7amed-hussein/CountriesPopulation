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
        public async void  AddMany(List<Country> data)
        {
            await _dbContext.Set<Country>().AddRangeAsync(data);
            _dbContext.SaveChanges();
        }
        public void Add(Country data)
        {
            _dbContext.Set<Country>().Add(data);
            _dbContext.SaveChanges();
        }
        public async Task Update(Country data)
        {
            var country = await _dbContext.Countries.Include(c=>c.PopulationCount).FirstOrDefaultAsync(c => c.Name == data.Name);
            if(country == null)
                return;
            country.Name = data.Name;
            country.Code = data.Code;
            country.Iso3 = data.Iso3;
            UpdatePopulation(country, data.PopulationCount);
            _dbContext.SaveChanges();
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
            _dbContext.SaveChanges();

        }
        public async Task<List<Country>> GetPagedList(int page)
        {
            return  await ( _dbContext.Countries.Include(c =>c.PopulationCount).AsNoTracking().Skip((page - 1) * 50).Take(50).ToListAsync());
        }
        public async Task<List<Country>> GetList()
        {
            return  await ( _dbContext.Countries.Include(c =>c.PopulationCount).AsNoTracking().ToListAsync());
        }
        public async Task<Country> Get(Guid id)
        {
            return await _dbContext.Countries.Include(c => c.PopulationCount).FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Country> Get(string search)
        {
            return await _dbContext.Countries.Include(c => c.PopulationCount).FirstOrDefaultAsync(c => c.Name == search || c.Code == search);
        }
    }
}

