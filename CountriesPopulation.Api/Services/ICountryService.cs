using CountriesPopulation.Api.Dtos;

namespace CountriesPopulation.Api.Services
{
    public interface ICountryService
    {
        void AddMany(List<CountryDto> data);
        Task<List<CountryDto>> GetList(int page);
        Task<CountryDto> Get(Guid id);
        Task<CountryDto> Get(string search);
        Task SyncFromApi(List<CountryDto> countriesDto);
    }
}