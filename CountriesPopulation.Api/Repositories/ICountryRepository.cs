using System;
namespace CountriesPopulation.Api.Repositories
{
    public interface ICountryRepository<TEntity>
    {
        Task AddManyAsync(List<TEntity> entity);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task<List<TEntity>> GetListAsync();
        Task<List<TEntity>> GetPagedListAsync(int page);
        Task<TEntity> GetAsync(Guid id);
        Task<TEntity> GetAsync(string search);
        Task SaveChangesAsync();
    }
}

